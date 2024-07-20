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
    private readonly ParameterSettings _parameterSettings = options.Value;

    #region Parameter Group CRUD
    public async Task<ParameterGroupWithParamsResult> AddParameterAsync(ParameterGroupWithParamsModel model, CancellationToken cancellationToken = default)
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
        foreach (var item in model.Parameters)
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
            await SetCacheAsync(result, cancellationToken);

        return result;
    }

    public async Task<ParameterGroupWithParamsResult?> UpdateParameterAsync(int parameterGroupId, ParameterGroupWithParamsModel model, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([parameterGroupId], cancellationToken);
        if (parameterGroup is not null)
        {
            if (parameterGroup.Description != model.Description || parameterGroup.Code != model.Code || parameterGroup.ApprovalRequired != model.ApprovalRequired)
            {
                parameterGroup.Description = model.Description;
                parameterGroup.ApprovalRequired = model.ApprovalRequired;
                parameterGroup.Code = model.Code;

                _parameterGroupRepository.Update(parameterGroup);
            }

            var parameterRepository = _parameterRepositoryResolver.GetParameterRepository(parameterGroup.ApprovalRequired);
            var parameters = await parameterRepository.FindAsync(c => c.GroupId == parameterGroupId, cancellationToken);
            var deleteParameters = from p in parameters
                                   join pm in model.Parameters on p.Id equals pm.Id
                                   into i
                                   from pm in i.DefaultIfEmpty()
                                   where pm is null
                                   select p;
            var insertParameters = from pm in model.Parameters
                                   join p in parameters on pm.Id equals p.Id
                                   into i
                                   from p in i.DefaultIfEmpty()
                                   where p is null
                                   select pm;
            var editParameters = from p in parameters
                                 join pm in model.Parameters on p.Id equals pm.Id
                                 where p.Order != pm.Order || p.Code != pm.Code || p.IsDefault != pm.IsDefault || p.Value != pm.Value
                                 select new { Current = p, Request = pm };

            foreach (var p in deleteParameters)
                parameterRepository.Remove(p);

            foreach (var p in insertParameters)
                await parameterRepository.AddAsync(new Parameter
                {
                    Code = p.Code,
                    Value = p.Value,
                    IsDefault = p.IsDefault,
                    Order = p.Order,
                    GroupId = parameterGroupId
                }, cancellationToken);

            foreach (var p in editParameters)
            {
                p.Current.Code = p.Request.Code;
                p.Current.Order = p.Request.Order;
                p.Current.IsDefault = p.Request.IsDefault;
                p.Current.Value = p.Request.Value;
                parameterRepository.Update(p.Current);
            }

            await _parameterGroupRepository.SaveChangesAsync(cancellationToken);

            var result = new ParameterGroupWithParamsResult
            {
                Code = model.Code,
                ApprovalRequired = model.ApprovalRequired,
                Description = model.Description,
                Id = parameterGroupId,
                Parameters = model.Parameters.Select(c => new ParameterResult
                {
                    Code = c.Code,
                    IsDefault = c.IsDefault,
                    Order = c.Order,
                    Id = c.Id,
                    Value = c.Value
                })
            };

            if (_parameterSettings.UseRedis)
            {
                await RemoveCacheAsync(parameterGroup, cancellationToken);
                await SetCacheAsync(result, cancellationToken);
            }
            return result;
        }

        throw new ParameterException("PR001", $"Not found parameter group (Id: {parameterGroupId}).");
    }

    public async Task<ParameterGroupResult> DeleteParameterAsync(int parameterGroupId, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([parameterGroupId], cancellationToken);
        if (parameterGroup is not null)
            _parameterGroupRepository.Remove(parameterGroup);
        else
            throw new ParameterException("PR001", $"Not found parameter group (Id: {parameterGroupId}).");

        await _parameterGroupRepository.SaveChangesAsync(cancellationToken);

        if (_parameterSettings.UseRedis)
            await RemoveCacheAsync(parameterGroup, cancellationToken);

        return parameterGroup.ToParameterGroupResult();
    }

    public async Task<ParameterGroupWithParamsResult?> GetParameterAsync(int parameterGroupId, CancellationToken cancellationToken = default)
    {
        if (_parameterSettings.UseRedis)
        {
            var cacheValue = await distributedCache.GetValueAsync($"{_parameterSettings.RedisPrefix}_Id:{parameterGroupId}", cancellationToken);
            if (cacheValue is not null)
                return cacheValue;
        }

        var result = await _parameterGroupRepository.GetParameterGroupWithParams(parameterGroupId, cancellationToken);

        if (_parameterSettings.UseRedis && result is not null)
            await SetCacheAsync(result, cancellationToken);

        return result;
    }

    public async Task<ParameterGroupWithParamsResult?> GetParameterAsync(string parameterGroupCode, CancellationToken cancellationToken = default)
    {
        if (_parameterSettings.UseRedis)
        {
            var cacheValue = await distributedCache.GetValueAsync($"{_parameterSettings.RedisPrefix}_Code:{parameterGroupCode}", cancellationToken);
            if (cacheValue is not null)
                return cacheValue;
        }

        var result = await _parameterGroupRepository.GetParameterGroupWithParams(parameterGroupCode, cancellationToken);

        if (_parameterSettings.UseRedis && result is not null)
            await SetCacheAsync(result, cancellationToken);

        return result;
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
    #endregion

    private async Task RemoveCacheAsync(ParameterGroup parameterGroup, CancellationToken cancellationToken)
    {
        await distributedCache.RemoveAsync($"{_parameterSettings.RedisPrefix}_Id:{parameterGroup.Id}", cancellationToken);
        await distributedCache.RemoveAsync($"{_parameterSettings.RedisPrefix}_Code:{parameterGroup.Code}", cancellationToken);
    }

    private async Task SetCacheAsync(ParameterGroupWithParamsResult result, CancellationToken cancellationToken)
    {
        await distributedCache.SetValueAsync(result, $"{_parameterSettings.RedisPrefix}_Id:{result.Id}", _parameterSettings.Time, cancellationToken);
        await distributedCache.SetValueAsync(result, $"{_parameterSettings.RedisPrefix}_Code:{result.Code}", _parameterSettings.Time, cancellationToken);
    }
}
