using CodeNet.Core;
using CodeNet.MakerChecker.Exception;
using CodeNet.MakerChecker.Models;
using CodeNet.MakerChecker.Repositories;
using System.Reflection;
using System.Threading;

namespace CodeNet.MakerChecker.Service;

internal class MakerCheckerManager<TDbContext>(TDbContext dbContext, ICodeNetContext identityContext) : IMakerCheckerManager
    where TDbContext : MakerCheckerDbContext
{
    private readonly MakerCheckerDefinitionRepository _makerCheckerDefinitionRepository = new(dbContext, identityContext);
    private readonly MakerCheckerFlowRepository _makerCheckerFlowRepository = new(dbContext, identityContext);
    private readonly MakerCheckerHistoryRepository _makerCheckerHistoryRepository = new(dbContext, identityContext);

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
            DefinitionId = flow.DefinitionId
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
            DefinitionId = flow.DefinitionId
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
            DefinitionId = result.DefinitionId,
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
            DefinitionId = result.DefinitionId,
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

    public async Task<TMakerCheckerEntity> ApproveAsync<TMakerCheckerEntity>(Guid referenceId, string description, CancellationToken cancellationToken = default)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        InternalMakerCheckerReposiyory<TMakerCheckerEntity> makerCheckerRepository = new(dbContext, identityContext);

        var entity = await makerCheckerRepository.GetByReferenceIdAsync(referenceId, cancellationToken) ?? throw new MakerCheckerException(ExceptionMessages.EntityNotFound);
        TMakerCheckerEntity? mainEntity = null;
        if (entity.MainReferenceId.HasValue)
            mainEntity = await makerCheckerRepository.GetByReferenceIdAsync(entity.MainReferenceId.Value, cancellationToken) ?? throw new MakerCheckerException(ExceptionMessages.EntityNotFound);

        if (entity.EntryState is EntryState.Update or EntryState.Delete && mainEntity is null)
            throw new MakerCheckerException(ExceptionMessages.EntityNotFound);

        if (entity.EntityStatus is not EntityStatus.Pending)
            throw new MakerCheckerException(ExceptionMessages.NoPendingData);

        var flowHistories = await makerCheckerRepository.GetMakerCheckerFlowHistoryListAsync(referenceId, cancellationToken);
        var flowHistory = flowHistories.FirstOrDefault(c => entity.Order == c.Order && c.MakerCheckerHistory.ApproveStatus == ApproveStatus.Pending) ?? throw new MakerCheckerException(ExceptionMessages.NoPendingFlow);

        var username = identityContext.UserName;
        var roles = identityContext.Roles;

        if (flowHistory.ApproveType is ApproveType.User && !flowHistory.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new MakerCheckerException(ExceptionMessages.Unauthorized);

        if (flowHistory.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.Approver)))
            throw new MakerCheckerException(ExceptionMessages.Unauthorized);

        flowHistory.MakerCheckerHistory.ApproveStatus = ApproveStatus.Approved;
        flowHistory.MakerCheckerHistory.Description = description;
        _makerCheckerHistoryRepository.Update(flowHistory.MakerCheckerHistory);

        if (flowHistories.All(c => c.MakerCheckerHistory.ApproveStatus is ApproveStatus.Approved))
        {
            entity.EntityStatus = EntityStatus.Completed;

            switch (entity.EntryState)
            {
                case EntryState.Insert:
                    entity.IsActive = true;
                    break;
                case EntryState.Update:
                    entity.IsActive = false;
                    Copy(mainEntity, entity);
                    makerCheckerRepository.InternalUpdate(mainEntity!);
                    break;
                case EntryState.Delete:
                    entity.IsActive = false;
                    makerCheckerRepository.InternalRemove(mainEntity!);
                    break;
                default:
                    break;
            }
        }
        else
        {
            var nextHistory = flowHistories.FirstOrDefault(c => c.Order > entity.Order);
            if (nextHistory is not null)
                entity.Order = nextHistory.Order;
        }
        makerCheckerRepository.InternalUpdate(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public TMakerCheckerEntity Approve<TMakerCheckerEntity>(Guid referenceId, string description)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        InternalMakerCheckerReposiyory<TMakerCheckerEntity> makerCheckerRepository = new(dbContext, identityContext);

        var entity = makerCheckerRepository.GetByReferenceId(referenceId) ?? throw new MakerCheckerException(ExceptionMessages.EntityNotFound);
        TMakerCheckerEntity? mainEntity = null;
        if (entity.MainReferenceId.HasValue)
            mainEntity = makerCheckerRepository.GetByReferenceId(entity.MainReferenceId.Value) ?? throw new MakerCheckerException(ExceptionMessages.EntityNotFound);

        if (entity.EntryState is EntryState.Update or EntryState.Delete && mainEntity is null)
            throw new MakerCheckerException(ExceptionMessages.EntityNotFound);

        if (entity.EntityStatus is not EntityStatus.Pending)
            throw new MakerCheckerException(ExceptionMessages.NoPendingData);

        var flowHistories = makerCheckerRepository.GetMakerCheckerFlowHistoryList(referenceId);
        var flowHistory = flowHistories.FirstOrDefault(c => entity.Order == c.Order && c.MakerCheckerHistory.ApproveStatus == ApproveStatus.Pending) ?? throw new MakerCheckerException(ExceptionMessages.NoPendingFlow);

        var username = identityContext.UserName;
        var roles = identityContext.Roles;

        if (flowHistory.ApproveType is ApproveType.User && !flowHistory.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new MakerCheckerException(ExceptionMessages.Unauthorized);

        if (flowHistory.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.Approver)))
            throw new MakerCheckerException(ExceptionMessages.Unauthorized);

        flowHistory.MakerCheckerHistory.ApproveStatus = ApproveStatus.Approved;
        flowHistory.MakerCheckerHistory.Description = description;
        _makerCheckerHistoryRepository.Update(flowHistory.MakerCheckerHistory);

        if (flowHistories.All(c => c.MakerCheckerHistory.ApproveStatus is ApproveStatus.Approved))
        {
            entity.EntityStatus = EntityStatus.Completed;

            switch (entity.EntryState)
            {
                case EntryState.Insert:
                    entity.IsActive = true;
                    break;
                case EntryState.Update:
                    entity.IsActive = false;
                    Copy(mainEntity, entity);
                    makerCheckerRepository.InternalUpdate(mainEntity!);
                    break;
                case EntryState.Delete:
                    entity.IsActive = false;
                    makerCheckerRepository.InternalRemove(mainEntity!);
                    break;
                default:
                    break;
            }
        }
        else
        {
            var nextHistory = flowHistories.FirstOrDefault(c => c.Order > entity.Order);
            if (nextHistory is not null)
                entity.Order = nextHistory.Order;
        }
        makerCheckerRepository.InternalUpdate(entity);
        dbContext.SaveChanges();

        return entity;
    }

    public async Task<TMakerCheckerEntity> RejectAsync<TMakerCheckerEntity>(Guid referenceId, string description, CancellationToken cancellationToken = default)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        InternalMakerCheckerReposiyory<TMakerCheckerEntity> makerCheckerRepository = new(dbContext, identityContext);
        var entity = await makerCheckerRepository.GetByReferenceIdAsync(referenceId, cancellationToken) ?? throw new MakerCheckerException(ExceptionMessages.EntityNotFound);

        if (entity.EntityStatus is not EntityStatus.Pending)
            throw new MakerCheckerException(ExceptionMessages.NoPendingData);

        var flowHistories = await makerCheckerRepository.GetMakerCheckerFlowHistoryListAsync(referenceId, cancellationToken);
        var flowHistory = flowHistories.FirstOrDefault(c => entity.Order == c.Order && c.MakerCheckerHistory.ApproveStatus == ApproveStatus.Pending) ?? throw new MakerCheckerException(ExceptionMessages.NoPendingFlow);

        if (flowHistories.Any(c => c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Rejected))
            throw new MakerCheckerException(ExceptionMessages.Rejected);

        var username = identityContext.UserName;
        var roles = identityContext.Roles;

        if (flowHistory.ApproveType is ApproveType.User && !flowHistory.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new MakerCheckerException(ExceptionMessages.Unauthorized);

        if (flowHistory.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.Approver)))
            throw new MakerCheckerException(ExceptionMessages.Unauthorized);

        entity.EntityStatus = EntityStatus.Rejected;

        foreach (var item in flowHistories.Where(c => c.Order > entity.Order))
        {
            item.MakerCheckerHistory.IsActive = false;
            _makerCheckerHistoryRepository.Update(item.MakerCheckerHistory);
        }
        flowHistory.MakerCheckerHistory.ApproveStatus = ApproveStatus.Rejected;
        flowHistory.MakerCheckerHistory.Description = description;
        _makerCheckerHistoryRepository.Update(flowHistory.MakerCheckerHistory);
        makerCheckerRepository.InternalUpdate(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public TMakerCheckerEntity Reject<TMakerCheckerEntity>(Guid referenceId, string description)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        InternalMakerCheckerReposiyory<TMakerCheckerEntity> makerCheckerRepository = new(dbContext, identityContext);
        var entity = makerCheckerRepository.GetByReferenceId(referenceId) ?? throw new MakerCheckerException(ExceptionMessages.EntityNotFound);

        if (entity.EntityStatus is not EntityStatus.Pending)
            throw new MakerCheckerException(ExceptionMessages.NoPendingData);

        var flowHistories = makerCheckerRepository.GetMakerCheckerFlowHistoryList(referenceId);
        var flowHistory = flowHistories.FirstOrDefault(c => entity.Order == c.Order && c.MakerCheckerHistory.ApproveStatus == ApproveStatus.Pending) ?? throw new MakerCheckerException(ExceptionMessages.NoPendingFlow);

        if (flowHistories.Any(c => c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Rejected))
            throw new MakerCheckerException(ExceptionMessages.Rejected);

        var username = identityContext.UserName;
        var roles = identityContext.Roles;

        if (flowHistory.ApproveType is ApproveType.User && !flowHistory.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new MakerCheckerException(ExceptionMessages.Unauthorized);

        if (flowHistory.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.Approver)))
            throw new MakerCheckerException(ExceptionMessages.Unauthorized);

        entity.EntityStatus = EntityStatus.Rejected;

        foreach (var item in flowHistories.Where(c => c.Order > entity.Order))
        {
            item.MakerCheckerHistory.IsActive = false;
            _makerCheckerHistoryRepository.Update(item.MakerCheckerHistory);
        }
        flowHistory.MakerCheckerHistory.ApproveStatus = ApproveStatus.Rejected;
        flowHistory.MakerCheckerHistory.Description = description;
        _makerCheckerHistoryRepository.Update(flowHistory.MakerCheckerHistory);
        makerCheckerRepository.InternalUpdate(entity);
        dbContext.SaveChanges();

        return entity;
    }

    private static TEntity Copy<TEntity>(TEntity source, TEntity destination)
    {
        var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var baseProperties = typeof(MakerCheckerEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties.Where(c => !baseProperties.Select(d => d.Name).Contains(c.Name)))
            property.SetValue(source, property.GetValue(destination));

        return source;
    }
}
