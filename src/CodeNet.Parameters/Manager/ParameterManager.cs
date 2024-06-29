using CodeNet.Core;
using CodeNet.Parameters.Exception;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using Pipelines.Sockets.Unofficial.Arenas;

namespace CodeNet.Parameters.Manager;

public class ParameterManager(ParametersDbContext dbContext, IIdentityContext identityContext) : IParameterManager
{
    private readonly ParameterMakerCheckerRepository _parameterMakerCheckerRepository = new(dbContext, identityContext);
    private readonly ParameterTracingRepository _parameterTracingRepository = new(dbContext, identityContext);
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);

    #region Parameter Group CRUD
    public ParameterGroupResult AddParameterGroup(AddParameterGroupModel model)
    {
        var parameterGroup = new ParameterGroup
        {
            Code = model.Code,
            ApprovalRequired = model.ApprovalRequired,
            Description = model.Description
        };
        var addResponse = _parameterGroupRepository.Add(parameterGroup);
        _parameterGroupRepository.SaveChanges();
        return ToParameterGroupResult(addResponse);
    }

    public ParameterGroupResult? GetParameterGroup(int parameterGroupId)
    {
        var parameterGroup = _parameterGroupRepository.Get(parameterGroupId);
        if (parameterGroup is null)
            return null;

        return ToParameterGroupResult(parameterGroup);
    }

    public ParameterGroupResult? GetParameterGroup(string parameterCode)
    {
        var parameterGroup = _parameterGroupRepository.Get(c => c.Code == parameterCode);
        if (parameterGroup is null)
            return null;

        return ToParameterGroupResult(parameterGroup);
    }

    public List<ParameterGroupResult> GetParameterGroupList(int page, int count)
    {
        return _parameterGroupRepository.GetPagingList(page, count).Select(c => new ParameterGroupResult
        {
            Code = c.Code,
            ApprovalRequired = c.ApprovalRequired,
            Description = c.Description,
            Id = c.Id
        }).ToList();
    }

    public ParameterGroupResult? UpdateParameterGroup(UpdateParameterGroupModel model)
    {
        var parameterGroup = _parameterGroupRepository.Get([model.Id]);
        if (parameterGroup is not null)
        {
            parameterGroup.Description = model.Description;
            parameterGroup.ApprovalRequired = model.ApprovalRequired;
            parameterGroup.Code = model.Code;

            var updateResponse = _parameterGroupRepository.Update(parameterGroup);
            _parameterGroupRepository.SaveChanges();
            return ToParameterGroupResult(updateResponse);
        }

        return null;
    }

    public bool DeleteParameterGroup(int parameterGroupId)
    {
        var parameterGroup = _parameterGroupRepository.Get(parameterGroupId);
        RemoveParameterGroup(parameterGroup, parameterGroupId);
        return _parameterGroupRepository.SaveChanges() > 0;
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
        return ToParameterGroupResult(addResponse);
    }

    public async Task<ParameterGroupResult?> GetParameterGroupAsync(int parameterGroupId, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([parameterGroupId], cancellationToken);
        if (parameterGroup is null)
            return null;

        return ToParameterGroupResult(parameterGroup);
    }

    public async Task<ParameterGroupResult?> GetParameterGroupAsync(string parameterCode, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync(c => c.Code == parameterCode, cancellationToken);
        if (parameterGroup is null)
            return null;

        return ToParameterGroupResult(parameterGroup);
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

    public async Task<ParameterGroupResult?> UpdateParameterGroupAsync(UpdateParameterGroupModel model, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([model.Id], cancellationToken);
        if (parameterGroup is not null)
        {
            parameterGroup.Description = model.Description;
            parameterGroup.ApprovalRequired = model.ApprovalRequired;
            parameterGroup.Code = model.Code;

            var updateResponse = _parameterGroupRepository.Update(parameterGroup);
            await _parameterGroupRepository.SaveChangesAsync(cancellationToken);
            return ToParameterGroupResult(updateResponse);
        }

        return null;
    }

    public async Task<bool> DeleteParameterGroupAsync(int parameterId, CancellationToken cancellationToken = default)
    {
        var parameterGroup = await _parameterGroupRepository.GetAsync([parameterId], cancellationToken);
        RemoveParameterGroup(parameterGroup, parameterId);
        return await _parameterGroupRepository.SaveChangesAsync(cancellationToken) > 0;
    }

    private void RemoveParameterGroup(ParameterGroup? parameterGroup, int parameterGroupId)
    {
        if (parameterGroup is not null)
            _parameterGroupRepository.Remove(parameterGroup);
        else
            throw new ParameterException("PR001", $"Not found parameter group (Id: {parameterGroupId}).");
    }

    private static ParameterGroupResult ToParameterGroupResult(ParameterGroup parameterGroup) => new()
    {
        ApprovalRequired = parameterGroup.ApprovalRequired,
        Code = parameterGroup.Code,
        Description = parameterGroup.Description,
        Id = parameterGroup.Id
    };
    #endregion

    #region Parameter CRUD
    public ParameterResult AddParameter(AddParameterModel model)
    {
        var approvalRequired = _parameterGroupRepository.GetApprovalRequired(model.GroupId);
        var parameterRepository = GetParameterRepository(approvalRequired);
        var parameter = new Parameter
        {
            Code = model.Code,
            GroupId = model.GroupId,
            Value = model.Value,
        };

        var addResponse = parameterRepository.Add(parameter);
        parameterRepository.SaveChanges();
        return ToParameterResult(addResponse);
    }

    public ParameterResult? GetParameter(int parameterId)
    {
        var parameter = _parameterTracingRepository.Get(parameterId);
        if (parameter is null)
            return null;

        return ToParameterResult(parameter);
    }

    public List<ParameterListItemResult> GetParameters(int groupId)
    {
        return _parameterGroupRepository.GetParameters(groupId);
    }
    public List<ParameterListItemResult> GetParameters(string groupCode)
    {
        return _parameterGroupRepository.GetParameters(groupCode);
    }

    public ParameterResult? UpdateParameter(UpdateParameterModel model)
    {
        var approvalRequired = _parameterGroupRepository.GetApprovalRequired(model.GroupId);
        var parameterRepository = GetParameterRepository(approvalRequired);
        var parameter = parameterRepository.Get([model.Id]);
        if (parameter is not null)
        {
            parameter.Value = model.Value;
            parameter.Code = model.Code;
            var updateResponse = parameterRepository.Update(parameter);
            parameterRepository.SaveChanges();
            return ToParameterResult(updateResponse);
        }

        return null;
    }

    public bool DeleteParameter(int parameterId)
    {
        var parameter = _parameterTracingRepository.Get(parameterId) ?? throw new ParameterException("PR001", $"Not found parameter (Id: {parameterId}).");
        var approvalRequired = _parameterGroupRepository.GetApprovalRequired(parameter.GroupId);
        var parameterRepository = RemoveParameter(parameter, parameterId, approvalRequired);
        return parameterRepository.SaveChanges() > 0;
    }

    public async Task<ParameterResult> AddParameterAsync(AddParameterModel model, CancellationToken cancellationToken = default)
    {
        var approvalRequired = await _parameterGroupRepository.GetApprovalRequiredAsync(model.GroupId, cancellationToken);
        var parameterRepository = GetParameterRepository(approvalRequired);
        var parameter = new Parameter
        {
            Code = model.Code,
            GroupId = model.GroupId,
            Value = model.Value,
        };

        var addResponse = await parameterRepository.AddAsync(parameter, cancellationToken);
        await parameterRepository.SaveChangesAsync(cancellationToken);
        return ToParameterResult(addResponse);
    }

    public async Task<ParameterResult?> GetParameterAsync(int parameterId, CancellationToken cancellationToken = default)
    {
        var parameter = await _parameterTracingRepository.GetAsync([parameterId], cancellationToken);
        if (parameter is null)
            return null;

        return ToParameterResult(parameter);
    }

    public Task<List<ParameterListItemResult>> GetParametersAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return _parameterGroupRepository.GetParametersAsync(groupId, cancellationToken);
    }
    public Task<List<ParameterListItemResult>> GetParametersAsync(string groupCode, CancellationToken cancellationToken = default)
    {
        return _parameterGroupRepository.GetParametersAsync(groupCode, cancellationToken);
    }

    public async Task<ParameterResult?> UpdateParameterAsync(UpdateParameterModel model, CancellationToken cancellationToken = default)
    {
        var approvalRequired = await _parameterGroupRepository.GetApprovalRequiredAsync(model.GroupId, cancellationToken);
        var parameterRepository = GetParameterRepository(approvalRequired);
        var parameter = await parameterRepository.GetAsync([model.Id], cancellationToken);
        if (parameter is not null)
        {
            parameter.Value = model.Value;
            parameter.Code = model.Code;
            var updateResponse = parameterRepository.Update(parameter);
            await parameterRepository.SaveChangesAsync(cancellationToken);
            return ToParameterResult(updateResponse);
        }

        return null;
    }

    public async Task<bool> DeleteParameterAsync(int parameterId, CancellationToken cancellationToken = default)
    {
        var parameter = await _parameterTracingRepository.GetAsync([parameterId], cancellationToken) ?? throw new ParameterException("PR001", $"Not found parameter (Id: {parameterId}).");
        var approvalRequired = await _parameterGroupRepository.GetApprovalRequiredAsync(parameter.GroupId, cancellationToken);
        var parameterRepository = RemoveParameter(parameter, parameterId, approvalRequired);
        return await parameterRepository.SaveChangesAsync(cancellationToken) > 0;
    }

    private IParameterRepository GetParameterRepository(bool approvelRequired) => approvelRequired ? _parameterMakerCheckerRepository : _parameterTracingRepository;

    private IParameterRepository RemoveParameter(Parameter parameter, int parameterId, bool approvalRequired)
    {
        var parameterRepository = GetParameterRepository(approvalRequired);
        parameterRepository.Remove(parameter);
        return parameterRepository;
    }

    private static ParameterResult ToParameterResult(Parameter parameter) => new()
    {
        Code = parameter.Code,
        GroupId = parameter.GroupId,
        Value = parameter.Value
    };
    #endregion
}
