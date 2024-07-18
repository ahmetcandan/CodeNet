using CodeNet.Core;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using CodeNet.Parameters.Settings;
using CodeNet.Redis;
using Microsoft.Extensions.Options;

namespace CodeNet.Parameters.Manager;

public class ParameterManager(ParametersDbContext dbContext, ICodeNetContext identityContext, IOptions<ParameterSettings> options, IDistributedCache<ParameterGroupWithParamsResult> distributedCache) : IParameterManager
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);
    private readonly ParameterRepositoryResolver _parameterRepositoryResolver = new(dbContext, identityContext);
    private readonly ParameterTracingRepository _parameterRepository = new(dbContext, identityContext);
    private readonly ParameterSettings _parameterSettings = options.Value;

    #region Parameter Group CRUD
    public async Task<ParameterGroupWithParamsResult> AddParameterGroupWithParamsAsync(AddParameterGroupWithParamsModel model, CancellationToken cancellationToken = default)
    {
        var parameterRepository = _parameterRepositoryResolver.GetParameterRepository(model.ApprovalRequired);
        var parameterGroup = new ParameterGroup
        {
            Code = model.Code,
            ApprovalRequired = model.ApprovalRequired,
            Description = model.Description
        };
        var addGroupResponse = await _parameterGroupRepository.AddAsync(parameterGroup, cancellationToken);
        var parameterResultList = new List<ParameterResult>();
        foreach (var item in model.AddParameters)
            parameterResultList.Add((await parameterRepository.AddAsync(new Parameter
            {
                Code = item.Code,
                Value = item.Value,
                GroupId = addGroupResponse.Id,
                IsDefault = item.IsDefault,
                Order = item.Order
            }, cancellationToken)).ToParameterResult());

        await _parameterGroupRepository.SaveChangesAsync(cancellationToken);

        var result = new ParameterGroupWithParamsResult
        {
            Code = addGroupResponse.Code,
            ApprovalRequired = addGroupResponse.ApprovalRequired,
            Description = addGroupResponse.Description,
            Id = addGroupResponse.Id,
            Parameters = parameterResultList
        };

        if (_parameterSettings.UseRedis)
        {
            await distributedCache.SetValueAsync(result, $"{_parameterSettings.RedisPrefix}_Id:{result.Id}", _parameterSettings.Time, cancellationToken);
            await distributedCache.SetValueAsync(result, $"{_parameterSettings.RedisPrefix}_Code:{result.Code}", _parameterSettings.Time, cancellationToken);
        }

        return result;
    }

    public async Task<ParameterGroupResult> AddParameterGroupAsync(AddParameterGroupModel model, CancellationToken cancellationToken = default)
    {
        var parameterGroup = new ParameterGroup
        {
            Code = model.Code,
            ApprovalRequired = model.ApprovalRequired,
            Description = model.Description
        };
        var addResponse = await _parameterGroupRepository.AddAsync(parameterGroup, cancellationToken);
        await _parameterGroupRepository.SaveChangesAsync(cancellationToken);
        return addResponse.ToParameterGroupResult();
    }

    public async Task<ParameterGroupResult?> GetParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([parameterGroupId], cancellationToken);
        return parameterGroup?.ToParameterGroupResult() ?? null;
    }

    public async Task<ParameterGroupResult?> GetParameterGroupAsync(string parameterCode, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([parameterCode], cancellationToken);
        return parameterGroup?.ToParameterGroupResult() ?? null;
    }

    public async Task<ParameterGroupWithParamsResult?> GetParameterGroupWithParamsAsync(int parameterGroupId, CancellationToken cancellationToken = default)
    {
        if (_parameterSettings.UseRedis)
        {
            var cacheValue = await distributedCache.GetValueAsync($"{_parameterSettings.RedisPrefix}_Id:{parameterGroupId}", cancellationToken);
            if (cacheValue is not null)
                return cacheValue;
        }

        return await _parameterGroupRepository.GetParameterGroupWithParams(parameterGroupId, cancellationToken);
    }

    public async Task<ParameterGroupWithParamsResult?> GetParameterGroupWithParamsAsync(string parameterGroupCode, CancellationToken cancellationToken = default)
    {
        if (_parameterSettings.UseRedis)
        {
            var cacheValue = await distributedCache.GetValueAsync($"{_parameterSettings.RedisPrefix}_Code:{parameterGroupCode}", cancellationToken);
            if (cacheValue is not null)
                return cacheValue;
        }

        return await _parameterGroupRepository.GetParameterGroupWithParams(parameterGroupCode, cancellationToken);
    }

    public async Task<List<ParameterGroupResult>> GetParameterGroupListAsync(int page, int count, CancellationToken cancellationToken = default)
    {
        return (await _parameterGroupRepository.GetPagingListAsync(page, count, cancellationToken)).Select(c => new ParameterGroupResult
        {
            Code = c.Code,
            ApprovalRequired = c.ApprovalRequired,
            Description = c.Description,
            Id = c.Id
        }).ToList();
    }

    public async Task<ParameterGroupResult> UpdateParameterGroupAsync(UpdateParameterGroupModel model, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([model.Id], cancellationToken);
        if (parameterGroup is not null)
        {
            parameterGroup.Description = model.Description;
            parameterGroup.ApprovalRequired = model.ApprovalRequired;
            parameterGroup.Code = model.Code;

            var updateResponse = _parameterGroupRepository.Update(parameterGroup);
            await _parameterGroupRepository.SaveChangesAsync(cancellationToken);
            await RemoveCacheAsync(parameterGroup, cancellationToken);
            return updateResponse.ToParameterGroupResult();
        }

        throw new ParameterException("PR001", $"Not found parameter group (Id: {model?.Id}).");
    }

    public async Task<ParameterGroupResult> DeleteParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([parameterGroupId], cancellationToken);
        if (parameterGroup is not null)
            _parameterGroupRepository.Remove(parameterGroup);
        else
            throw new ParameterException("PR001", $"Not found parameter group (Id: {parameterGroupId}).");
        await _parameterGroupRepository.SaveChangesAsync(cancellationToken);

        await RemoveCacheAsync(parameterGroup, cancellationToken);
        return parameterGroup.ToParameterGroupResult();
    }
    #endregion

    #region Parameter CRUD
    public async Task<ParameterResult> AddParameterAsync(AddParameterModel model, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetParameterGroupAsync(model.GroupId, cancellationToken) ?? throw new ParameterException("PR001", $"Not found parameter group (Id: {model.GroupId}).");
        var parameterRepository = _parameterRepositoryResolver.GetParameterRepository(parameterGroup.ApprovalRequired);
        var parameter = new Parameter
        {
            Code = model.Code,
            GroupId = model.GroupId,
            Value = model.Value,
            IsDefault = model.IsDefault,
            Order = model.Order
        };

        var addResponse = await parameterRepository.AddAsync(parameter, cancellationToken);
        await parameterRepository.SaveChangesAsync(cancellationToken);
        await RemoveCacheAsync(parameterGroup, cancellationToken);

        return addResponse.ToParameterResult();
    }

    public async Task<ParameterResult?> GetParameterAsync(int parameterId, CancellationToken cancellationToken = default)
    {
        var parameter = await _parameterRepository.GetAsync([parameterId], cancellationToken);
        return parameter?.ToParameterResult();
    }

    public Task<List<ParameterListItemResult>> GetParametersAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return _parameterGroupRepository.GetParametersAsync(groupId, cancellationToken);
    }

    public Task<List<ParameterListItemResult>> GetParametersAsync(string groupCode, CancellationToken cancellationToken = default)
    {
        return _parameterGroupRepository.GetParametersAsync(groupCode, cancellationToken);
    }

    public async Task<ParameterResult> UpdateParameterAsync(UpdateParameterModel model, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetParameterGroupAsync(model.GroupId, cancellationToken) ?? throw new ParameterException("PR001", $"Not found parameter group (Id: {model.GroupId}).");
        var parameterRepository = _parameterRepositoryResolver.GetParameterRepository(parameterGroup.ApprovalRequired);
        var parameter = await parameterRepository.GetAsync([model.Id], cancellationToken);
        if (parameter is not null)
        {
            parameter.Value = model.Value;
            parameter.Code = model.Code;
            parameter.IsDefault = model.IsDefault;
            parameter.Order = model.Order;

            var updateResponse = parameterRepository.Update(parameter);
            await parameterRepository.SaveChangesAsync(cancellationToken);
            await RemoveCacheAsync(parameterGroup, cancellationToken);
            return updateResponse.ToParameterResult();
        }

        throw new ParameterException("PR002", $"Not found parameter (Id: {model?.Id}).");
    }

    public async Task<ParameterResult> DeleteParameterAsync(int parameterId, CancellationToken cancellationToken = default)
    {
        var parameter = await _parameterRepository.GetAsync([parameterId], cancellationToken) ?? throw new ParameterException("PR002", $"Not found parameter (Id: {parameterId}).");
        var parameterGroup = await _parameterGroupRepository.GetParameterGroupAsync(parameter.GroupId, cancellationToken) ?? throw new ParameterException("PR001", $"Not found parameter group (Id: {parameter.GroupId}).");
        var parameterRepository = _parameterRepositoryResolver.GetParameterRepository(parameterGroup.ApprovalRequired);
        parameterRepository.Remove(parameter);
        await parameterRepository.SaveChangesAsync(cancellationToken);
        await RemoveCacheAsync(parameterGroup, cancellationToken);
        return parameter.ToParameterResult();
    }

    private async Task RemoveCacheAsync(ParameterGroup parameterGroup, CancellationToken cancellationToken)
    {
        if (!_parameterSettings.UseRedis)
            return;

        await distributedCache.RemoveAsync($"{_parameterSettings.RedisPrefix}_Id:{parameterGroup.Id}", cancellationToken);
        await distributedCache.RemoveAsync($"{_parameterSettings.RedisPrefix}_Code:{parameterGroup.Code}", cancellationToken);
    }
    #endregion
}
