using CodeNet.Core;
using CodeNet.ExceptionHandling;
using CodeNet.MakerChecker.Models;
using CodeNet.MakerChecker.Repositories;

namespace CodeNet.MakerChecker;

public class MakerCheckerManager(MakerCheckerDbContext dbContext, IIdentityContext identityContext) : IMakerCheckerManager
{
    private readonly MakerCheckerDefinitionRepository _makerCheckerDefinitionRepository = new(dbContext, identityContext);
    private readonly MakerCheckerFlowRepository _makerCheckerFlowRepository = new(dbContext, identityContext);

    public Guid InsertDefinition(DefinitionInserModel definition)
    {
        var result = _makerCheckerDefinitionRepository.Add(new MakerCheckerDefinition
        {
            EntityName = definition.EntityName,
            Id = Guid.NewGuid()
        });
        return result.Id;
    }

    public async Task<Guid> InsertDefinitionAsync(DefinitionInserModel definition, CancellationToken cancellationToken = default)
    {
        var result = await _makerCheckerDefinitionRepository.AddAsync(new MakerCheckerDefinition
        {
            EntityName = definition.EntityName,
            Id = Guid.NewGuid()
        }, cancellationToken);
        return result.Id;
    }

    public DefinitionUpdateModel UpdateDefinition(DefinitionUpdateModel definition)
    {
        var updateModel = _makerCheckerDefinitionRepository.Get(definition.Id) ?? throw new UserLevelException("MC007", "No records found to update.");
        updateModel.EntityName = definition.EntityName;
        _makerCheckerDefinitionRepository.Update(updateModel);
        return definition;
    }

    public async Task<DefinitionUpdateModel> UpdateDefinitionAsync(DefinitionUpdateModel definition, CancellationToken cancellationToken = default)
    {
        var updateModel = await _makerCheckerDefinitionRepository.GetAsync([definition.Id], cancellationToken) ?? throw new UserLevelException("MC007", "No records found to update.");
        updateModel.EntityName = definition.EntityName;
        _makerCheckerDefinitionRepository.Update(updateModel);
        return definition;
    }

    public DefinitionUpdateModel DeleteDefinition(Guid definitionId)
    {
        var updateModel = _makerCheckerDefinitionRepository.Get(definitionId) ?? throw new UserLevelException("MC009", "No records found to delete.");
        var result = _makerCheckerDefinitionRepository.Remove(updateModel);
        return new DefinitionUpdateModel
        {
            EntityName = result.EntityName,
            Id = result.Id
        };
    }

    public async Task<DefinitionUpdateModel> DeleteDefinitionAsync(Guid definitionId, CancellationToken cancellationToken = default)
    {
        var updateModel = await _makerCheckerDefinitionRepository.GetAsync([definitionId], cancellationToken) ?? throw new UserLevelException("MC009", "No records found to delete.");
        var result = _makerCheckerDefinitionRepository.Remove(updateModel);
        return new DefinitionUpdateModel
        {
            EntityName = result.EntityName,
            Id = result.Id
        };
    }

    public Guid InsertFlow(FlowInserModel flow)
    {
        var result = _makerCheckerFlowRepository.Add(new MakerCheckerFlow
        {
            Approver = flow.Approver,
            ApproveType = flow.ApproveType,
            Id = Guid.NewGuid(),
            Order = flow.Order,
            MakerCheckerDefinitionId = flow.DefinitionId
        });
        return result.Id;
    }

    public async Task<Guid> InsertFlowAsync(FlowInserModel flow)
    {
        var result = await _makerCheckerFlowRepository.AddAsync(new MakerCheckerFlow
        {
            Approver = flow.Approver,
            ApproveType = flow.ApproveType,
            Id = Guid.NewGuid(),
            Order = flow.Order,
            MakerCheckerDefinitionId = flow.DefinitionId
        });
        return result.Id;
    }

    public FlowUpdateModel UpdateFlow(FlowUpdateModel flow)
    {
        var updateModel = _makerCheckerFlowRepository.Get(flow.Id) ?? throw new UserLevelException("MC008", "No records found to update.");
        updateModel.Approver = flow.Approver;
        updateModel.ApproveType = flow.ApproveType;
        _makerCheckerFlowRepository.Update(updateModel);
        return flow;
    }

    public async Task<FlowUpdateModel> UpdateFlowAsync(FlowUpdateModel flow, CancellationToken cancellationToken = default)
    {
        var updateModel = await _makerCheckerFlowRepository.GetAsync([flow.Id], cancellationToken) ?? throw new UserLevelException("MC008", "No records found to update.");
        updateModel.Approver = flow.Approver;
        updateModel.ApproveType = flow.ApproveType;
        _makerCheckerFlowRepository.Update(updateModel);
        return flow;
    }

    public FlowUpdateModel DeleteFlow(Guid flowId)
    {
        var updateModel = _makerCheckerFlowRepository.Get(flowId) ?? throw new UserLevelException("MC010", "No records found to delete.");
        var result = _makerCheckerFlowRepository.Remove(updateModel);
        return new FlowUpdateModel
        {
            Approver = result.Approver,
            ApproveType = result.ApproveType,
            Id = flowId,
            DefinitionId = result.MakerCheckerDefinitionId,
            Order = result.Order
        };
    }

    public async Task<FlowUpdateModel> DeleteFlowAsync(Guid flowId, CancellationToken cancellationToken = default)
    {
        var updateModel = await _makerCheckerFlowRepository.GetAsync([flowId], cancellationToken) ?? throw new UserLevelException("MC010", "No records found to delete.");
        var result = _makerCheckerFlowRepository.Remove(updateModel);
        return new FlowUpdateModel
        {
            Approver = result.Approver,
            ApproveType = result.ApproveType,
            Id = flowId,
            DefinitionId = result.MakerCheckerDefinitionId,
            Order = result.Order
        };
    }

    public List<MakerCheckerPending> GetPendingList()
    {
        return _makerCheckerFlowRepository.GetPendingList();
    }

    public Task<List<MakerCheckerPending>> GetPendingListAsync(CancellationToken cancellationToken = default)
    {
        return _makerCheckerFlowRepository.GetPendingListAsync(cancellationToken);
    }
}
