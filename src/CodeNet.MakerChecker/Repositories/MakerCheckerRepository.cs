using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Exception;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Repositories;

public abstract class MakerCheckerRepository<TMakerCheckerEntity> : TracingRepository<TMakerCheckerEntity>, IMakerCheckerRepository<TMakerCheckerEntity>
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    private readonly IIdentityContext _identityContext;
    private readonly string _entityName = typeof(TMakerCheckerEntity).Name;
    private readonly DbSet<MakerCheckerDefinition> _makerCheckerDefinitions;
    private readonly DbSet<MakerCheckerFlow> _makerCheckerFlows;
    private readonly DbSet<MakerCheckerHistory> _makerCheckerHistories;
    private readonly MakerCheckerHistoryRepository _makerCheckerHistoryRepository;
    private readonly MakerCheckerDraftEntityRepository _makerCheckerDraftEntityRepository;

    public MakerCheckerRepository(MakerCheckerDbContext dbContext, IIdentityContext identityContext) : base(dbContext, identityContext)
    {
        _identityContext = identityContext;
        _makerCheckerDefinitions = _dbContext.Set<MakerCheckerDefinition>();
        _makerCheckerFlows = _dbContext.Set<MakerCheckerFlow>();
        _makerCheckerHistories = _dbContext.Set<MakerCheckerHistory>();
        _makerCheckerHistoryRepository = new MakerCheckerHistoryRepository(dbContext, identityContext);
        _makerCheckerDraftEntityRepository = new MakerCheckerDraftEntityRepository(dbContext, identityContext);
    }

    public override TMakerCheckerEntity Add(TMakerCheckerEntity entity)
    {
        return MakerCheckerStart(entity, EntryState.Insert);
    }

    public override Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity)
    {
        return AddAsync(entity, CancellationToken.None);
    }

    public override Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken)
    {
        return MakerCheckerStartAsync(entity, EntryState.Insert, cancellationToken);
    }

    public override TMakerCheckerEntity Update(TMakerCheckerEntity entity)
    {
        return MakerCheckerStart(entity, EntryState.Update);
    }

    public TMakerCheckerEntity? Approve(MakerCheckerDraftEntity draft, string description)
    {
        var approveStatus = ApproveEntity(draft, description, [.. GetMakerCheckerFlowHistoryListQueryable(draft.Id)]);
        if (approveStatus is ApproveStatus.Approved)
            return base.Add(draft.GetEntity<TMakerCheckerEntity>() ?? throw new MakerCheckerException("MC012", "Convert exceptipon"));

        return null;
    }

    public async Task<TMakerCheckerEntity?> ApproveAsync(MakerCheckerDraftEntity draft, string description, CancellationToken cancellationToken = default)
    {
        var approveStatus = ApproveEntity(draft, description, await GetMakerCheckerFlowHistoryListQueryable(draft.Id).ToListAsync(cancellationToken));
        if (approveStatus is ApproveStatus.Approved)
            return await base.AddAsync(draft.GetEntity<TMakerCheckerEntity>() ?? throw new MakerCheckerException("MC012", "Convert exceptipon"), cancellationToken);

        return null;
    }

    public void Reject(MakerCheckerDraftEntity draft, string description)
    {
        RejectEntity(draft, description, [.. GetMakerCheckerFlowHistoryListQueryable(draft.Id)]);
    }

    public async Task RejectAsync(MakerCheckerDraftEntity draft, string description, CancellationToken cancellationToken = default)
    {
        RejectEntity(draft, description, await GetMakerCheckerFlowHistoryListQueryable(draft.Id).ToListAsync(cancellationToken));
    }

    public virtual MakerCheckerDraftEntity? GetDraft(Guid referenceId)
    {
        return _makerCheckerDraftEntityRepository.Get(c => c.Id == referenceId);
    }

    public virtual Task<MakerCheckerDraftEntity?> GetDraftAsync(Guid referenceId, CancellationToken cancellationToken = default)
    {
        return _makerCheckerDraftEntityRepository.GetAsync(c => c.Id == referenceId, cancellationToken);
    }

    private TMakerCheckerEntity MakerCheckerStart(TMakerCheckerEntity entity, EntryState entryState)
    {
        var flows = GetMakerCheckerFlowListQueryable().ToList();
        if (flows.Count == 0)
            throw new MakerCheckerException("MC008", "MakerCheckerFlow not found");

        var draft = MakerCheckerRepository<TMakerCheckerEntity>.NewDraftEntity(entity, entryState, flows[0].Order);
        _makerCheckerDraftEntityRepository.Add(draft);

        foreach (var flow in flows)
            _makerCheckerHistoryRepository.Add(NewHistory(flow, draft.Id));

        entity.ReferenceId = draft.Id;
        return entity;
    }

    private async Task<TMakerCheckerEntity> MakerCheckerStartAsync(TMakerCheckerEntity entity, EntryState entryState, CancellationToken cancellationToken)
    {
        var flows = await GetMakerCheckerFlowListQueryable().ToListAsync(cancellationToken);
        if (flows.Count == 0)
            throw new MakerCheckerException("MC008", "MakerCheckerFlow not found");

        var draft = MakerCheckerRepository<TMakerCheckerEntity>.NewDraftEntity(entity, entryState, flows[0].Order);
        await _makerCheckerDraftEntityRepository.AddAsync(draft, cancellationToken);

        foreach (var flow in flows)
            await _makerCheckerHistoryRepository.AddAsync(NewHistory(flow, draft.Id), cancellationToken);

        entity.ReferenceId = draft.Id;
        return entity;
    }

    private ApproveStatus ApproveEntity(MakerCheckerDraftEntity draft, string description, List<MakerCheckerFlowHistory> flowHistories)
    {
        var flowHistory = flowHistories.FirstOrDefault(c => draft.Order == c.Order && c.MakerCheckerHistory.ApproveStatus == ApproveStatus.Pending) ?? throw new MakerCheckerException("MC001", "No records found to approve.");

        var username = _identityContext.UserName;
        var roles = _identityContext.Roles;

        if (flowHistory.ApproveType is ApproveType.User && !flowHistory.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new MakerCheckerException("MC002", "Approve cannot be made with this user.");

        if (flowHistory.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.Approver)))
            throw new MakerCheckerException("MC003", "Approve cannot be made with this user/role.");

        flowHistory.MakerCheckerHistory.ApproveStatus = ApproveStatus.Approved;
        flowHistory.MakerCheckerHistory.Description = description;
        _makerCheckerHistoryRepository.Update(flowHistory.MakerCheckerHistory);

        if (flowHistories.All(c => c.MakerCheckerHistory.ApproveStatus is ApproveStatus.Approved))
            draft.SetApproveStatus(ApproveStatus.Approved);
        else
        {
            var nextHistory = flowHistories.FirstOrDefault(c => c.Order > draft.Order);
            if (nextHistory is not null)
                draft.SetOrder(nextHistory.Order);
        }

        _makerCheckerDraftEntityRepository.Update(draft);
        return draft.ApproveStatus;
    }

    private ApproveStatus RejectEntity(MakerCheckerDraftEntity draft, string description, List<MakerCheckerFlowHistory> flowHistories)
    {
        if (draft.ApproveStatus is ApproveStatus.Approved)
            throw new MakerCheckerException("MC011", "This is an approved data.");

        if (draft.ApproveStatus is ApproveStatus.Rejected || flowHistories.Any(c => c.MakerCheckerHistory?.ApproveStatus == ApproveStatus.Rejected))
            throw new MakerCheckerException("MC004", "This registration was previously rejected.");

        var flowHistory = flowHistories.FirstOrDefault(c => c.Order == draft.Order && c.MakerCheckerHistory.ApproveStatus == ApproveStatus.Pending) ?? throw new MakerCheckerException("MC005", "No record found to reject.");

        var username = _identityContext.UserName;
        var roles = _identityContext.Roles;

        if (flowHistory.ApproveType is ApproveType.User && !flowHistory.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
            throw new MakerCheckerException("MC005", "Reject cannot be made with this user.");

        if (flowHistory.ApproveType is ApproveType.Role && !roles.Any(r => r.Equals(flowHistory.Approver)))
            throw new MakerCheckerException("MC006", "Reject cannot be made with this user/role.");

        draft.SetApproveStatus(ApproveStatus.Rejected);

        foreach (var item in flowHistories)
        {
            if (item.Equals(flowHistory))
                continue;

            item.MakerCheckerHistory.IsActive = false;
            _makerCheckerHistoryRepository.Update(item.MakerCheckerHistory);
        }
        flowHistory.MakerCheckerHistory.ApproveStatus = ApproveStatus.Rejected;
        flowHistory.MakerCheckerHistory.Description = description;
        _makerCheckerHistoryRepository.Update(flowHistory.MakerCheckerHistory);

        _makerCheckerDraftEntityRepository.Update(draft);
        return draft.ApproveStatus;
    }

    private static MakerCheckerDraftEntity NewDraftEntity(TMakerCheckerEntity entity, EntryState entryState, byte order)
    {
        var draft = entity.NewDraft(entryState, order);
        draft.SetEntity(entity);
        return draft;
    }

    private IQueryable<MakerCheckerFlowHistory> GetMakerCheckerFlowHistoryListQueryable(Guid referenceId)
    {
        return (from definition in _makerCheckerDefinitions
                join flow in _makerCheckerFlows on definition.Id equals flow.MakerCheckerDefinitionId
                join history in _makerCheckerHistories on flow.Id equals history.MakerCheckerFlowId
                where definition.EntityName == _entityName
                    && history.ReferenceId == referenceId
                    && definition.IsActive && !definition.IsDeleted
                    && flow.IsActive && !flow.IsDeleted
                    && history.IsActive && !history.IsDeleted
                orderby flow.Order ascending
                select new MakerCheckerFlowHistory { MakerCheckerHistory = history, Approver = flow.Approver, ApproveType = flow.ApproveType, Order = flow.Order, EntityName = definition.EntityName });
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

    private static MakerCheckerHistory NewHistory(MakerCheckerFlow flow, Guid referenceId) => new()
    {
        Id = Guid.NewGuid(),
        ApproveStatus = ApproveStatus.Pending,
        MakerCheckerFlowId = flow.Id,
        ReferenceId = referenceId
    };
}
