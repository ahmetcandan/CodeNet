using CodeNet.Core.Context;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Repositories;

internal class MakerCheckerFlowRepository : TracingRepository<MakerCheckerFlow>
{
    private readonly DbSet<MakerCheckerFlow> _makerCheckerFlows;
    private readonly DbSet<MakerCheckerHistory> _makerCheckerHistories;

    public MakerCheckerFlowRepository(MakerCheckerDbContext makerCheckerDbContext, ICodeNetContext identityContext) : base(makerCheckerDbContext, identityContext)
    {
        _makerCheckerFlows = _dbContext.Set<MakerCheckerFlow>();
        _makerCheckerHistories = _dbContext.Set<MakerCheckerHistory>();
    }

    public List<MakerCheckerPending> GetPendingList() => [.. GetPendingListQueryable()];

    public Task<List<MakerCheckerPending>> GetPendingListAsync(CancellationToken cancellationToken = default) => GetPendingListQueryable().ToListAsync(cancellationToken);

    private IQueryable<MakerCheckerPending> GetPendingListQueryable()
        => (from flow in _makerCheckerFlows
            join history in _makerCheckerHistories on flow.Id equals history.FlowId
            where flow.IsActive && !flow.IsDeleted
              && history.IsActive && !history.IsDeleted
              && history.ApproveStatus == ApproveStatus.Pending
            select new MakerCheckerPending { ReferenceId = history.ReferenceId, History = history, Flow = flow })
                .AsNoTracking();
}
