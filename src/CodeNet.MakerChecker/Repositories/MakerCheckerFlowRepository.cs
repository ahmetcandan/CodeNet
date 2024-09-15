using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Repositories;

internal class MakerCheckerFlowRepository : TracingRepository<MakerCheckerFlow>
{
    private readonly ICodeNetContext _identityContext;
    private readonly DbSet<MakerCheckerDefinition> _makerCheckerDefinitions;
    private readonly DbSet<MakerCheckerFlow> _makerCheckerFlows;
    private readonly DbSet<MakerCheckerHistory> _makerCheckerHistories;

    public MakerCheckerFlowRepository(MakerCheckerDbContext makerCheckerDbContext, ICodeNetContext identityContext) : base(makerCheckerDbContext, identityContext)
    {
        _identityContext = identityContext;
        _makerCheckerDefinitions = _dbContext.Set<MakerCheckerDefinition>();
        _makerCheckerFlows = _dbContext.Set<MakerCheckerFlow>();
        _makerCheckerHistories = _dbContext.Set<MakerCheckerHistory>();
    }

    public List<MakerCheckerPending> GetPendingList()
    {
        return [.. GetPendingListQueryable()];
    }

    public Task<List<MakerCheckerPending>> GetPendingListAsync(CancellationToken cancellationToken = default)
    {
        return GetPendingListQueryable().ToListAsync(cancellationToken);
    }

    private IQueryable<MakerCheckerPending> GetPendingListQueryable()
    {
        var username = _identityContext.UserName;
        var roles = _identityContext.Roles.ToList();
        return (from definition in _makerCheckerDefinitions
                join flow in _makerCheckerFlows on definition.Id equals flow.DefinitionId
                join history in _makerCheckerHistories on flow.Id equals history.FlowId
                where definition.IsActive && !definition.IsDeleted
                  && flow.IsActive && !flow.IsDeleted
                  && history.IsActive && !history.IsDeleted
                  && history.ApproveStatus == ApproveStatus.Pending
                select new MakerCheckerPending { ReferenceId = history.ReferenceId, History = history, EntityName = definition.EntityName, Flow = flow })
                .AsNoTracking();
    }
}
