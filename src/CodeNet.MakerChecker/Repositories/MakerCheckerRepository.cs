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
        foreach (var flow in flows)
            _makerCheckerHistoryRepository.Add(NewHistory(flow, entity.ReferenceId));

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
        await _makerCheckerHistoryRepository.AddRangeAsync(flows.Select(f => NewHistory(f, entity.ReferenceId)), cancellationToken);

        return await base.AddAsync(entity, cancellationToken);
    }

    public override TMakerCheckerEntity Update(TMakerCheckerEntity entity)
    {
        MakerCheckerRepository<TMakerCheckerEntity>.EntityResetStatus(entity);

        var flows = GetMakerCheckerFlowListQueryable().ToList();
        _makerCheckerHistoryRepository.AddRange(flows.Select(f => NewHistory(f, entity.ReferenceId)));

        return base.Update(entity);
    }

    public void Approve(TMakerCheckerEntity entity)
    {
        var flowHistories = GetMakerCheckerFlowHistoryListQueryable(entity.ReferenceId).ToList();
        var history = ApproveEntity(entity, flowHistories);
        _makerCheckerHistoryRepository.Add(history);
    }

    public async Task ApproveAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken = default)
    {
        var flowHistories = await GetMakerCheckerFlowHistoryListQueryable(entity.ReferenceId).ToListAsync(cancellationToken);
        var history = ApproveEntity(entity, flowHistories);
        await _makerCheckerHistoryRepository.AddAsync(history, cancellationToken);
    }

    public void Reject(TMakerCheckerEntity entity)
    {
        var flowHistories = GetMakerCheckerFlowHistoryListQueryable(entity.ReferenceId).ToList();
        var history = RejectEntity(entity, flowHistories);
        _makerCheckerHistoryRepository.Add(history);
    }

    public async Task RejectAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken = default)
    {
        var flowHistories = await GetMakerCheckerFlowHistoryListQueryable(entity.ReferenceId).ToListAsync(cancellationToken);
        var history = RejectEntity(entity, flowHistories);
        await _makerCheckerHistoryRepository.AddAsync(history, cancellationToken);
    }

    private MakerCheckerHistory ApproveEntity(TMakerCheckerEntity entity, List<MakerCheckerFlowHistory> flowHistories)
    {
        var flowHistory = flowHistories.FirstOrDefault(c => c.MakerCheckerHistory is null || c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Pending) ?? throw new UserLevelException("MC001", "No records found to approve.");
        var username = _identityContext.UserName;
        var roles = _identityContext.Roles;

        if (flowHistory.MakerCheckerFlow.ApproveType is ApproveType.User && !flowHistory.MakerCheckerFlow.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new UserLevelException("MC002", "Approve cannot be made with this user.");

        if (flowHistory.MakerCheckerFlow.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.MakerCheckerFlow.Approver)))
            throw new UserLevelException("MC003", "Approve cannot be made with this user.");

        var approvedCount = flowHistories.Count(c => c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Approved);
        if (approvedCount + 1 >= flowHistories.Count)
            entity.SetApproveStatus(ApproveStatus.Approved);

        return new()
        {
            Id = Guid.NewGuid(),
            MakerCheckerFlowId = flowHistory.MakerCheckerFlow.Id,
            ReferenceId = entity.ReferenceId,
            ApproveStatus = ApproveStatus.Approved
        };
    }

    private MakerCheckerHistory RejectEntity(TMakerCheckerEntity entity, List<MakerCheckerFlowHistory> flowHistories)
    {
        if (flowHistories.Any(c => c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Rejected))
            throw new UserLevelException("MC004", "This registration was previously rejected.");

        var flowHistory = flowHistories.FirstOrDefault(c => c.MakerCheckerHistory is null || c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Pending) ?? throw new UserLevelException("MC005", "No record found to reject.");
        var username = _identityContext.UserName;
        var roles = _identityContext.Roles;

        if (flowHistory.MakerCheckerFlow.ApproveType is ApproveType.User && !flowHistory.MakerCheckerFlow.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new UserLevelException("MC005", "Reject cannot be made with this user.");

        if (flowHistory.MakerCheckerFlow.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.MakerCheckerFlow.Approver)))
            throw new UserLevelException("MC006", "Reject cannot be made with this user.");

        var approvedCount = flowHistories.Count(c => c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Approved);
        if (approvedCount + 1 >= flowHistories.Count)
            entity.SetApproveStatus(ApproveStatus.Approved);

        return new()
        {
            Id = Guid.NewGuid(),
            MakerCheckerFlowId = flowHistory.MakerCheckerFlow.Id,
            ReferenceId = entity.ReferenceId,
            ApproveStatus = ApproveStatus.Rejected
        };
    }

    private IQueryable<MakerCheckerFlowHistory> GetMakerCheckerFlowHistoryListQueryable(Guid referenceId)
    {
        return (from definition in _makerCheckerDefinitions
                join flow in _makerCheckerFlows on definition.Id equals flow.MakerCheckerDefinitionId
                join history in _makerCheckerHistories on flow.Id equals history.MakerCheckerFlowId
                where definition.EntityName == _entityName && history.ReferenceId == referenceId
                    && flow.IsActive && !flow.IsDeleted
                    && definition.IsActive && !definition.IsDeleted
                    && history.IsActive && !history.IsDeleted
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
                select flow);
    }

    private static void EntityResetStatus(TMakerCheckerEntity entity)
    {
        entity.SetNewReferenceId();
        entity.SetApproveStatus(ApproveStatus.Pending);
    }

    private static MakerCheckerHistory NewHistory(MakerCheckerFlow flow, Guid referenceId) => new()
    {
        Id = Guid.NewGuid(),
        ApproveStatus = ApproveStatus.Pending,
        MakerCheckerFlowId = flow.Id,
        ReferenceId = referenceId
    };

    public override Task<List<TMakerCheckerEntity>> FindAsync(Expression<Func<TMakerCheckerEntity, bool>> predicate)
    {
        return FindAsync(predicate, CancellationToken.None);
    }

    public override Task<List<TMakerCheckerEntity>> FindAsync(Expression<Func<TMakerCheckerEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return FindAsync(predicate, true, false, cancellationToken);
    }

    public override Task<List<TMakerCheckerEntity>> FindAsync(Expression<Func<TMakerCheckerEntity, bool>> predicate, bool isActive = true, bool isDeleted = false, CancellationToken cancellationToken = default)
    {
        return base.FindAsync(AddCondition(c => c.ApproveStatus == ApproveStatus.Approved, predicate), isActive, isDeleted, cancellationToken);
    }

    public override List<TMakerCheckerEntity> Find(Expression<Func<TMakerCheckerEntity, bool>> predicate)
    {
        return Find(predicate, true, false);
    }

    public override List<TMakerCheckerEntity> Find(Expression<Func<TMakerCheckerEntity, bool>> predicate, bool isActive = true, bool isDeleted = false)
    {
        return base.Find(AddCondition(c => c.ApproveStatus == ApproveStatus.Approved, predicate), isActive, isDeleted);
    }

    public virtual List<TMakerCheckerEntity> FindByStatus(Expression<Func<TMakerCheckerEntity, bool>> predicate, ApproveStatus approveStatus)
    {
        return base.Find(AddCondition(c => c.ApproveStatus == approveStatus, predicate), true, false);
    }

    public virtual Task<List<TMakerCheckerEntity>> FindByStatusAsync(Expression<Func<TMakerCheckerEntity, bool>> predicate, ApproveStatus approveStatus, CancellationToken cancellationToken = default)
    {
        return base.FindAsync(AddCondition(c => c.ApproveStatus == approveStatus, predicate), true, false, cancellationToken);
    }
}
