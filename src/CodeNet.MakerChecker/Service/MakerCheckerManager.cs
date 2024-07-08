using CodeNet.Core;
using CodeNet.MakerChecker.Exception;
using CodeNet.MakerChecker.Models;
using CodeNet.MakerChecker.Repositories;

namespace CodeNet.MakerChecker;

internal class MakerCheckerManager(MakerCheckerDbContext dbContext, ICodeNetContext identityContext) : IMakerCheckerManager
{
    private readonly MakerCheckerDefinitionRepository _makerCheckerDefinitionRepository = new(dbContext, identityContext);
    private readonly MakerCheckerFlowRepository _makerCheckerFlowRepository = new(dbContext, identityContext);

    public Guid InsertDefinition<TMakerCheckerEntity>()
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        var result = _makerCheckerDefinitionRepository.Add(new MakerCheckerDefinition
        {
            EntityName = typeof(TMakerCheckerEntity).Name,
            Id = Guid.NewGuid()
        });
        _makerCheckerDefinitionRepository.SaveChanges();
        return result.Id;
    }

    public async Task<Guid> InsertDefinitionAsync<TMakerCheckerEntity>(CancellationToken cancellationToken = default)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        var result = await _makerCheckerDefinitionRepository.AddAsync(new MakerCheckerDefinition
        {
            EntityName = typeof(TMakerCheckerEntity).Name,
            Id = Guid.NewGuid()
        }, cancellationToken);
        await _makerCheckerDefinitionRepository.SaveChangesAsync(cancellationToken);
        return result.Id;
    }

    public DefinitionUpdateModel UpdateDefinition<TMakerCheckerEntity>(DefinitionUpdateModel definition)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        var updateModel = _makerCheckerDefinitionRepository.Get(definition.Id) ?? throw new MakerCheckerException("MC007", "No records found to update.");
        updateModel.EntityName = typeof(TMakerCheckerEntity).Name;
        _makerCheckerDefinitionRepository.Update(updateModel);
        _makerCheckerDefinitionRepository.SaveChanges();
        return definition;
    }

    public async Task<DefinitionUpdateModel> UpdateDefinitionAsync<TMakerCheckerEntity>(DefinitionUpdateModel definition, CancellationToken cancellationToken = default)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        var updateModel = await _makerCheckerDefinitionRepository.GetAsync([definition.Id], cancellationToken) ?? throw new MakerCheckerException("MC007", "No records found to update.");
        updateModel.EntityName = typeof(TMakerCheckerEntity).Name;
        _makerCheckerDefinitionRepository.Update(updateModel);
        await _makerCheckerDefinitionRepository.SaveChangesAsync(cancellationToken);
        return definition;
    }

    public DefinitionUpdateModel DeleteDefinition(Guid definitionId)
    {
        var updateModel = _makerCheckerDefinitionRepository.Get(definitionId) ?? throw new MakerCheckerException("MC009", "No records found to delete.");
        var result = _makerCheckerDefinitionRepository.Remove(updateModel);
        _makerCheckerDefinitionRepository.SaveChanges();
        return new DefinitionUpdateModel
        {
            Id = result.Id
        };
    }

    public async Task<DefinitionUpdateModel> DeleteDefinitionAsync(Guid definitionId, CancellationToken cancellationToken = default)
    {
        var updateModel = await _makerCheckerDefinitionRepository.GetAsync([definitionId], cancellationToken) ?? throw new MakerCheckerException("MC009", "No records found to delete.");
        var result = _makerCheckerDefinitionRepository.Remove(updateModel);
        await _makerCheckerDefinitionRepository.SaveChangesAsync(cancellationToken);
        return new DefinitionUpdateModel
        {
            Id = result.Id
        };
    }

    public Guid InsertFlow(FlowInserModel flow)
    {
        var result = _makerCheckerFlowRepository.Add(new MakerCheckerFlow
        {
            Approver = flow.Approver,
            ApproveType = flow.ApproveType,
            Description = flow.Description,
            Id = Guid.NewGuid(),
            Order = flow.Order,
            MakerCheckerDefinitionId = flow.DefinitionId
        });
        _makerCheckerFlowRepository.SaveChanges();
        return result.Id;
    }

    public async Task<Guid> InsertFlowAsync(FlowInserModel flow, CancellationToken cancellationToken = default)
    {
        var result = await _makerCheckerFlowRepository.AddAsync(new MakerCheckerFlow
        {
            Approver = flow.Approver,
            ApproveType = flow.ApproveType,
            Description = flow.Description,
            Id = Guid.NewGuid(),
            Order = flow.Order,
            MakerCheckerDefinitionId = flow.DefinitionId
        }, cancellationToken);
        await _makerCheckerFlowRepository.SaveChangesAsync(cancellationToken);
        return result.Id;
    }

    public FlowUpdateModel UpdateFlow(FlowUpdateModel flow)
    {
        var updateModel = _makerCheckerFlowRepository.Get(flow.Id) ?? throw new MakerCheckerException("MC008", "No records found to update.");
        updateModel.Approver = flow.Approver;
        updateModel.ApproveType = flow.ApproveType;
        updateModel.Description = flow.Description;
        _makerCheckerFlowRepository.Update(updateModel);
        _makerCheckerFlowRepository.SaveChanges();
        return flow;
    }

    public async Task<FlowUpdateModel> UpdateFlowAsync(FlowUpdateModel flow, CancellationToken cancellationToken = default)
    {
        var updateModel = await _makerCheckerFlowRepository.GetAsync([flow.Id], cancellationToken) ?? throw new MakerCheckerException("MC008", "No records found to update.");
        updateModel.Approver = flow.Approver;
        updateModel.ApproveType = flow.ApproveType;
        updateModel.Description = flow.Description;
        _makerCheckerFlowRepository.Update(updateModel);
        await _makerCheckerFlowRepository.SaveChangesAsync(cancellationToken);
        return flow;
    }

    public FlowUpdateModel DeleteFlow(Guid flowId)
    {
        var updateModel = _makerCheckerFlowRepository.Get(flowId) ?? throw new MakerCheckerException("MC010", "No records found to delete.");
        var result = _makerCheckerFlowRepository.Remove(updateModel);
        _makerCheckerFlowRepository.SaveChanges();
        return new FlowUpdateModel
        {
            Approver = result.Approver,
            ApproveType = result.ApproveType,
            Id = flowId,
            Description = result.Description,
            DefinitionId = result.MakerCheckerDefinitionId,
            Order = result.Order
        };
    }

    public async Task<FlowUpdateModel> DeleteFlowAsync(Guid flowId, CancellationToken cancellationToken = default)
    {
        var updateModel = await _makerCheckerFlowRepository.GetAsync([flowId], cancellationToken) ?? throw new MakerCheckerException("MC010", "No records found to delete.");
        var result = _makerCheckerFlowRepository.Remove(updateModel);
        await _makerCheckerFlowRepository.SaveChangesAsync(cancellationToken);
        return new FlowUpdateModel
        {
            Approver = result.Approver,
            ApproveType = result.ApproveType,
            Id = flowId,
            Description = result.Description,
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
