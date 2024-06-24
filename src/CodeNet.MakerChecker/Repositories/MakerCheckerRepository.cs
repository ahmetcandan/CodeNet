using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.ExceptionHandling;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodeNet.MakerChecker.Repositories;

public abstract class MakerCheckerRepository<TMakerCheckerEntity> : TracingRepository<TMakerCheckerEntity>, IMakerCheckerRepository<TMakerCheckerEntity>
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    private readonly DbSet<MakerCheckerDefinition> _makerCheckerDefinitions;
    private readonly DbSet<MakerCheckerFlow> _makerCheckerFlows;
    private readonly DbSet<MakerCheckerHistory> _makerCheckerHistories;
    private readonly string _entityName = typeof(TMakerCheckerEntity).Name;
    private readonly MakerCheckerHistoryRepository _makerCheckerHistoryRepository;
    private readonly IIdentityContext _identityContext;

    public MakerCheckerRepository(MakerCheckerDbContext dbContext, IIdentityContext identityContext) : base(dbContext, identityContext)
    {
        _identityContext = identityContext;
        _makerCheckerDefinitions = _dbContext.Set<MakerCheckerDefinition>();
        _makerCheckerFlows = _dbContext.Set<MakerCheckerFlow>();
        _makerCheckerHistories = _dbContext.Set<MakerCheckerHistory>();
        _makerCheckerHistoryRepository = new MakerCheckerHistoryRepository(dbContext, identityContext);
    }

    public override TMakerCheckerEntity Add(TMakerCheckerEntity entity)
    {
        MakerCheckerRepository<TMakerCheckerEntity>.EntityResetStatus(entity);

        var flows = GetMakerCheckerFlowListQueryable().ToList();
        for (int i = 0; i < flows.Count; i++)
            _makerCheckerHistoryRepository.Add(NewHistory(flows[i], entity.ReferenceId, i == 0));

        return base.Add(entity);
    }

    public override Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity)
    {
        return AddAsync(entity, CancellationToken.None);
    }

    public override async Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken)
    {
        MakerCheckerRepository<TMakerCheckerEntity>.EntityResetStatus(entity);

        var flows = await GetMakerCheckerFlowListQueryable().ToListAsync(cancellationToken);
        for (int i = 0; i < flows.Count; i++)
            _makerCheckerHistoryRepository.Add(NewHistory(flows[i], entity.ReferenceId, i == 0));

        return await base.AddAsync(entity, cancellationToken);
    }

    public override TMakerCheckerEntity Update(TMakerCheckerEntity entity)
    {
        MakerCheckerRepository<TMakerCheckerEntity>.EntityResetStatus(entity);

        var flows = GetMakerCheckerFlowListQueryable().ToList();
        for (int i = 0; i < flows.Count; i++)
            _makerCheckerHistoryRepository.Add(NewHistory(flows[i], entity.ReferenceId, i == 0));

        return base.Update(entity);
    }

    public void Approve(TMakerCheckerEntity entity)
    {
        var flowHistories = GetMakerCheckerFlowHistoryListQueryable(entity.ReferenceId).ToList();
        ApproveEntity(entity, flowHistories);
    }

    public async Task ApproveAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken = default)
    {
        var flowHistories = await GetMakerCheckerFlowHistoryListQueryable(entity.ReferenceId).ToListAsync(cancellationToken);
        ApproveEntity(entity, flowHistories);
    }

    public void Reject(TMakerCheckerEntity entity)
    {
        var flowHistories = GetMakerCheckerFlowHistoryListQueryable(entity.ReferenceId).ToList();
        RejectEntity(entity, flowHistories);
    }

    public async Task RejectAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken = default)
    {
        var flowHistories = await GetMakerCheckerFlowHistoryListQueryable(entity.ReferenceId).ToListAsync(cancellationToken);
        RejectEntity(entity, flowHistories);
    }

    private void ApproveEntity(TMakerCheckerEntity entity, List<MakerCheckerFlowHistory> flowHistories)
    {
        var flowHistory = flowHistories.FirstOrDefault(c => c.MakerCheckerHistory.ApproveStatus == ApproveStatus.Pending && c.MakerCheckerHistory.IsActive) ?? throw new UserLevelException("MC001", "No records found to approve.");
        var username = _identityContext.UserName;
        var roles = _identityContext.Roles;

        if (flowHistory.MakerCheckerFlow.ApproveType is ApproveType.User && !flowHistory.MakerCheckerFlow.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new UserLevelException("MC002", "Approve cannot be made with this user.");

        if (flowHistory.MakerCheckerFlow.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.MakerCheckerFlow.Approver)))
            throw new UserLevelException("MC003", "Approve cannot be made with this user.");

        var approvedCount = flowHistories.Count(c => c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Approved);
        if (approvedCount + 1 >= flowHistories.Count)
            entity.SetApproveStatus(ApproveStatus.Approved);

        var nextHistory = flowHistories.FirstOrDefault(c => c.MakerCheckerFlow.Order > flowHistory.MakerCheckerFlow.Order)?.MakerCheckerHistory;
        if (nextHistory is not null)
        {
            nextHistory.IsActive = true;
            _makerCheckerHistoryRepository.Update(nextHistory);
        }

        flowHistory.MakerCheckerHistory.IsActive = false;
        flowHistory.MakerCheckerHistory.ApproveStatus = ApproveStatus.Approved;
        _makerCheckerHistoryRepository.Update(flowHistory.MakerCheckerHistory);
    }

    private void RejectEntity(TMakerCheckerEntity entity, List<MakerCheckerFlowHistory> flowHistories)
    {
        if (flowHistories.Any(c => c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Rejected))
            throw new UserLevelException("MC004", "This registration was previously rejected.");

        var flowHistory = flowHistories.FirstOrDefault(c => c.MakerCheckerHistory.ApproveStatus == ApproveStatus.Pending && c.MakerCheckerHistory.IsActive) ?? throw new UserLevelException("MC005", "No record found to reject.");
        var username = _identityContext.UserName;
        var roles = _identityContext.Roles;

        if (flowHistory.MakerCheckerFlow.ApproveType is ApproveType.User && !flowHistory.MakerCheckerFlow.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new UserLevelException("MC005", "Reject cannot be made with this user.");

        if (flowHistory.MakerCheckerFlow.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.MakerCheckerFlow.Approver)))
            throw new UserLevelException("MC006", "Reject cannot be made with this user.");

        entity.SetApproveStatus(ApproveStatus.Rejected);
        foreach (var item in flowHistories)
        {
            item.MakerCheckerHistory.IsActive = false;
            item.MakerCheckerHistory.ApproveStatus = ApproveStatus.Rejected;
            _makerCheckerHistoryRepository.Update(item.MakerCheckerHistory);
        }
    }

    private IQueryable<MakerCheckerFlowHistory> GetMakerCheckerFlowHistoryListQueryable(Guid referenceId)
    {
        return (from definition in _makerCheckerDefinitions
                join flow in _makerCheckerFlows on definition.Id equals flow.MakerCheckerDefinitionId
                join history in _makerCheckerHistories on flow.Id equals history.MakerCheckerFlowId
                where definition.EntityName == _entityName && history.ReferenceId == referenceId
                    && flow.IsActive && !flow.IsDeleted
                    && definition.IsActive && !definition.IsDeleted
                    && !history.IsDeleted
                select new MakerCheckerFlowHistory { MakerCheckerFlow = flow, MakerCheckerHistory = history })
                .Distinct()
                .OrderBy(c => c.MakerCheckerFlow.Order);
    }

    private IQueryable<MakerCheckerFlow> GetMakerCheckerFlowListQueryable()
    {
        return (from definition in _makerCheckerDefinitions
                join flow in _makerCheckerFlows on definition.Id equals flow.MakerCheckerDefinitionId
                where definition.EntityName == _entityName
                    && definition.IsActive && !definition.IsDeleted
                    && flow.IsActive && !flow.IsDeleted
                orderby flow.Order
                select flow);
    }

    private static void EntityResetStatus(TMakerCheckerEntity entity)
    {
        entity.SetNewReferenceId();
        entity.SetApproveStatus(ApproveStatus.Pending);
    }

    private static MakerCheckerHistory NewHistory(MakerCheckerFlow flow, Guid referenceId, bool isActive) => new()
    {
        Id = Guid.NewGuid(),
        ApproveStatus = ApproveStatus.Pending,
        MakerCheckerFlowId = flow.Id,
        ReferenceId = referenceId,
        IsActive = isActive
    };

    public override Task<List<TMakerCheckerEntity>> FindAsync(Expression<Func<TMakerCheckerEntity, bool>> predicate)
    {
        return FindAsync(predicate, CancellationToken.None);
    }

    public override Task<List<TMakerCheckerEntity>> FindAsync(Expression<Func<TMakerCheckerEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return FindAsync(predicate, true, cancellationToken);
    }

    public override Task<List<TMakerCheckerEntity>> FindAsync(Expression<Func<TMakerCheckerEntity, bool>> predicate, bool isActive = true, CancellationToken cancellationToken = default)
    {
        return FindByStatusAsync(predicate, ApproveStatus.Approved, isActive);
    }

    public override List<TMakerCheckerEntity> Find(Expression<Func<TMakerCheckerEntity, bool>> predicate)
    {
        return Find(predicate, true);
    }

    public override List<TMakerCheckerEntity> Find(Expression<Func<TMakerCheckerEntity, bool>> predicate, bool isActive = true)
    {
        return FindByStatus(predicate, ApproveStatus.Approved, isActive);
    }

    public virtual List<TMakerCheckerEntity> FindByStatus(Expression<Func<TMakerCheckerEntity, bool>> predicate, ApproveStatus approveStatus, bool isActive = true)
    {
        return base.Find(AddCondition(c => c.ApproveStatus == approveStatus, predicate), isActive);
    }

    public virtual Task<List<TMakerCheckerEntity>> FindByStatusAsync(Expression<Func<TMakerCheckerEntity, bool>> predicate, ApproveStatus approveStatus, bool isActive = true, CancellationToken cancellationToken = default)
    {
        return base.FindAsync(AddCondition(c => c.ApproveStatus == approveStatus, predicate), isActive, cancellationToken);
    }

    public virtual TMakerCheckerEntity? GetByReferenceId(Guid referenceId, ApproveStatus approveStatus, bool isActive = true)
    {
        return _entities.FirstOrDefault(c => c.ReferenceId == referenceId && c.IsActive == isActive && !c.IsDeleted && c.ApproveStatus == approveStatus);
    }

    public virtual Task<TMakerCheckerEntity?> GetByReferenceIdAsync(Guid referenceId, ApproveStatus approveStatus, bool isActive = true, CancellationToken cancellationToken = default)
    {
        return _entities.FirstOrDefaultAsync(c => c.ReferenceId == referenceId && c.IsActive == isActive && !c.IsDeleted && c.ApproveStatus == approveStatus, cancellationToken);
    }
}
