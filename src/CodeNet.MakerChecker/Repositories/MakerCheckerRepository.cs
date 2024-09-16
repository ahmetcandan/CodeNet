using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Exception;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CodeNet.MakerChecker.Repositories;

public abstract class MakerCheckerRepository<TMakerCheckerEntity> : TracingRepository<TMakerCheckerEntity>, IMakerCheckerRepository<TMakerCheckerEntity>
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    private readonly string _entityName = typeof(TMakerCheckerEntity).Name;
    private readonly DbSet<MakerCheckerDefinition> _makerCheckerDefinitions;
    private readonly DbSet<MakerCheckerFlow> _makerCheckerFlows;
    private readonly DbSet<MakerCheckerHistory> _makerCheckerHistories;
    private readonly MakerCheckerHistoryRepository _makerCheckerHistoryRepository;

    public MakerCheckerRepository(MakerCheckerDbContext dbContext, ICodeNetContext identityContext) : base(dbContext, identityContext)
    {
        _makerCheckerDefinitions = _dbContext.Set<MakerCheckerDefinition>();
        _makerCheckerFlows = _dbContext.Set<MakerCheckerFlow>();
        _makerCheckerHistories = _dbContext.Set<MakerCheckerHistory>();
        _makerCheckerHistoryRepository = new MakerCheckerHistoryRepository(dbContext, identityContext);
    }

    public override TMakerCheckerEntity Add(TMakerCheckerEntity entity)
    {
        return Add(entity, EntryState.Insert);
    }

    private TMakerCheckerEntity Add(TMakerCheckerEntity entity, EntryState entryState, Guid? mainReferenceId = null)
    {
        entity.EntityStatus = EntityStatus.Pending;
        entity.ReferenceId = Guid.NewGuid();
        entity.MainReferenceId = mainReferenceId;
        entity.EntryState = entryState;
        entity.IsActive = false;

        var flows = GetMakerCheckerFlowListQueryable().ToList();
        if (flows.Count is 0)
            throw new MakerCheckerException(ExceptionMessages.FlowNotFound);

        foreach (var flow in flows)
            _makerCheckerHistoryRepository.Add(NewHistory(flow, entity.ReferenceId));

        entity.Order = flows.Min(x => x.Order);
        if (entryState is EntryState.Update or EntryState.Delete)
        {
            var properties = typeof(TMakerCheckerEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(c => c.GetCustomAttribute<AutoIncrementAttribute>() is not null || c.GetCustomAttribute<Models.PrimaryKeyAttribute>() is not null);
            foreach (var property in properties)
                property.SetValue(entity, null);
        }

        return base.Add(entity);
    }

    private async Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity, EntryState entryState, Guid? mainReferenceId = null, CancellationToken cancellationToken = default)
    {
        entity.EntityStatus = EntityStatus.Pending;
        entity.ReferenceId = Guid.NewGuid();
        entity.MainReferenceId = mainReferenceId;
        entity.EntryState = entryState;
        entity.IsActive = false;

        var flows = await GetMakerCheckerFlowListQueryable().ToListAsync(cancellationToken);
        if (flows.Count is 0)
            throw new MakerCheckerException(ExceptionMessages.FlowNotFound);

        foreach (var flow in flows)
            await _makerCheckerHistoryRepository.AddAsync(NewHistory(flow, entity.ReferenceId), cancellationToken);

        entity.Order = flows.Min(x => x.Order);
        if (entryState is EntryState.Update or EntryState.Delete)
        {
            var properties = typeof(TMakerCheckerEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(c => c.GetCustomAttribute<AutoIncrementAttribute>() is not null || c.GetCustomAttribute<Models.PrimaryKeyAttribute>() is not null);
            foreach (var property in properties)
                property.SetValue(entity, null);
        }

        return await base.AddAsync(entity, cancellationToken);
    }

    public override Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity)
    {
        return AddAsync(entity, CancellationToken.None);
    }

    public override Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken)
    {
        return AddAsync(entity, EntryState.Insert, cancellationToken);
    }

    private async Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity, EntryState entryState, CancellationToken cancellationToken)
    {
        entity.EntityStatus = EntityStatus.Pending;
        entity.ReferenceId = Guid.NewGuid();
        entity.EntryState = entryState;
        entity.IsActive = false;

        var flows = await GetMakerCheckerFlowListQueryable().ToListAsync(cancellationToken);
        if (flows.Count is 0)
            throw new MakerCheckerException(ExceptionMessages.FlowNotFound);

        foreach (var flow in flows)
            await _makerCheckerHistoryRepository.AddAsync(NewHistory(flow, entity.ReferenceId), cancellationToken);

        entity.Order = flows.Min(x => x.Order);
        return await base.AddAsync(entity, cancellationToken);
    }

    public override TMakerCheckerEntity Update(TMakerCheckerEntity entity)
    {
        return Add(entity, EntryState.Update, entity.ReferenceId);
    }

    public Task<TMakerCheckerEntity> UpdateAsync(TMakerCheckerEntity entity)
    {
        return UpdateAsync(entity, CancellationToken.None);
    }

    public Task<TMakerCheckerEntity> UpdateAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken)
    {
        return AddAsync(entity, EntryState.Update, entity.ReferenceId, cancellationToken);
    }

    public override TMakerCheckerEntity Remove(TMakerCheckerEntity entity)
    {
        return Add(entity, EntryState.Delete, entity.ReferenceId);
    }
    
    public Task<TMakerCheckerEntity> RemoveAsync(TMakerCheckerEntity entity)
    {
        return RemoveAsync(entity, CancellationToken.None);
    }

    public Task<TMakerCheckerEntity> RemoveAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken)
    {
        return AddAsync(entity, EntryState.Delete, entity.ReferenceId, cancellationToken);
    }

    public virtual TMakerCheckerEntity? GetByReferenceId(Guid referenceId)
    {
        return _entities.FirstOrDefault(c => c.ReferenceId == referenceId);
    }

    public virtual Task<TMakerCheckerEntity?> GetByReferenceIdAsync(Guid referenceId, CancellationToken cancellationToken = default)
    {
        return _entities.FirstOrDefaultAsync(c => c.ReferenceId == referenceId, cancellationToken);
    }

    private IQueryable<MakerCheckerFlow> GetMakerCheckerFlowListQueryable()
    {
        return (from definition in _makerCheckerDefinitions
                join flow in _makerCheckerFlows on definition.Id equals flow.DefinitionId
                where definition.EntityName == _entityName
                    && definition.IsActive && !definition.IsDeleted
                    && flow.IsActive && !flow.IsDeleted
                orderby flow.Order
                select flow)
                .AsNoTracking();
    }

    private static MakerCheckerHistory NewHistory(MakerCheckerFlow flow, Guid referenceId) => new()
    {
        Id = Guid.NewGuid(),
        ApproveStatus = ApproveStatus.Pending,
        FlowId = flow.Id,
        ReferenceId = referenceId
    };

    internal IQueryable<MakerCheckerFlowHistory> GetMakerCheckerFlowHistoryListQueryable(Guid referenceId)
    {
        return from definition in _makerCheckerDefinitions
               join flow in _makerCheckerFlows on definition.Id equals flow.DefinitionId
               join history in _makerCheckerHistories on flow.Id equals history.FlowId
               where definition.EntityName == _entityName
                   && history.ReferenceId == referenceId
                   && definition.IsActive && !definition.IsDeleted
                   && flow.IsActive && !flow.IsDeleted
                   && history.IsActive && !history.IsDeleted
               orderby flow.Order ascending
               select new MakerCheckerFlowHistory { MakerCheckerHistory = history, Approver = flow.Approver, ApproveType = flow.ApproveType, Order = flow.Order, EntityName = definition.EntityName };
    }

    protected internal TMakerCheckerEntity InternalUpdate(TMakerCheckerEntity entity)
    {
        return base.Update(entity);
    }

    protected internal TMakerCheckerEntity InternalRemove(TMakerCheckerEntity entity)
    {
        return base.Remove(entity);
    }

    #region Obsolete
    [Obsolete("This method is not used for this 'MakerCheckerRepository'")]
    public override TMakerCheckerEntity HardDelete(TMakerCheckerEntity entity)
    {
        throw new MakerCheckerException(ExceptionMessages.MethodNotUsed.UseParams("HardDelete"));
    }

    [Obsolete("This method is not used for this 'MakerCheckerRepository'")]
    public override IEnumerable<TMakerCheckerEntity> HardDeleteRange(IEnumerable<TMakerCheckerEntity> entities)
    {
        throw new MakerCheckerException(ExceptionMessages.MethodNotUsed.UseParams("HardDeleteRange"));
    }

    [Obsolete("This method is not used for this 'MakerCheckerRepository'")]
    public override IEnumerable<TMakerCheckerEntity> RemoveRange(IEnumerable<TMakerCheckerEntity> entities)
    {
        throw new MakerCheckerException(ExceptionMessages.MethodNotUsed.UseParams("RemoveRange"));
    }

    [Obsolete("This method is not used for this 'MakerCheckerRepository'")]
    public override IEnumerable<TMakerCheckerEntity> AddRange(IEnumerable<TMakerCheckerEntity> entities)
    {
        throw new MakerCheckerException(ExceptionMessages.MethodNotUsed.UseParams("AddRange"));
    }

    [Obsolete("This method is not used for this 'MakerCheckerRepository'")]
    public override Task<IEnumerable<TMakerCheckerEntity>> AddRangeAsync(IEnumerable<TMakerCheckerEntity> entities)
    {
        throw new MakerCheckerException(ExceptionMessages.MethodNotUsed.UseParams("AddRangeAsync"));
    }

    [Obsolete("This method is not used for this 'MakerCheckerRepository'")]
    public override Task<IEnumerable<TMakerCheckerEntity>> AddRangeAsync(IEnumerable<TMakerCheckerEntity> entities, CancellationToken cancellationToken)
    {
        throw new MakerCheckerException(ExceptionMessages.MethodNotUsed.UseParams("AddRangeAsync"));
    }

    [Obsolete("This method is not used for this 'MakerCheckerRepository'")]
    public override IEnumerable<TMakerCheckerEntity> UpdateRange(IEnumerable<TMakerCheckerEntity> entities)
    {
        throw new MakerCheckerException(ExceptionMessages.MethodNotUsed.UseParams("UpdateRange"));
    }
    #endregion
}
