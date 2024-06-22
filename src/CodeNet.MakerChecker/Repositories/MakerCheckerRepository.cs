using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.ExceptionHandling;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Repositories;

public class MakerCheckerRepository<TMakerCheckerEntity> : TracingRepository<TMakerCheckerEntity>, IMakerCheckerRepository<TMakerCheckerEntity>
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
        return base.Add(entity);
    }

    public override Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity)
    {
        return AddAsync(entity, CancellationToken.None);
    }

    public override async Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken)
    {
        MakerCheckerRepository<TMakerCheckerEntity>.EntityResetStatus(entity);
        return await base.AddAsync(entity, cancellationToken);
    }

    public override TMakerCheckerEntity Update(TMakerCheckerEntity entity)
    {
        MakerCheckerRepository<TMakerCheckerEntity>.EntityResetStatus(entity);
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
                join history in _makerCheckerHistories.Where(h => h.IsActive && !h.IsDeleted && h.ReferenceId == referenceId) on flow.Id equals history.MakerCheckerFlowId
                    into historyGroup
                from history in historyGroup.DefaultIfEmpty()
                where definition.EntityName == _entityName
                    && flow.IsActive && !flow.IsDeleted
                    && definition.IsActive && !definition.IsDeleted
                select new MakerCheckerFlowHistory { MakerCheckerFlow = flow, MakerCheckerHistory = history })
                .Distinct()
                .OrderBy(c => c.MakerCheckerFlow.Order);
    }

    private static void EntityResetStatus(TMakerCheckerEntity entity)
    {
        entity.SetNewReferenceId();
        entity.SetApproveStatus(ApproveStatus.Pending);
    }
}
