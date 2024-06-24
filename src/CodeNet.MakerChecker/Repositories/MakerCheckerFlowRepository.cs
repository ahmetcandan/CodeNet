using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Repositories;

internal class MakerCheckerFlowRepository : TracingRepository<MakerCheckerFlow>
{
    private readonly IIdentityContext _identityContext;
    private readonly DbSet<MakerCheckerDefinition> _makerCheckerDefinitions;
    private readonly DbSet<MakerCheckerFlow> _makerCheckerFlows;
    private readonly DbSet<MakerCheckerHistory> _makerCheckerHistories;

    public MakerCheckerFlowRepository(MakerCheckerDbContext makerCheckerDbContext, IIdentityContext identityContext) : base(makerCheckerDbContext, identityContext)
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
        var roles = _identityContext.Roles;
        return (from definition in _makerCheckerDefinitions
                      join flow in _makerCheckerFlows on definition.Id equals flow.MakerCheckerDefinitionId
                      join history in _makerCheckerHistories on flow.Id equals history.MakerCheckerFlowId
                      where definition.IsActive && !definition.IsDeleted 
                        && flow.IsActive && !flow.IsDeleted 
                        && history.IsActive && !history.IsDeleted
                      select new { Definition = definition, Flow = flow, History = history })
                      .GroupBy(c => c.History.ReferenceId)
                      .Select(c =>
                          new MakerCheckerPending
                          {
                              ReferenceId = c.Key,
                              EntityName = c.OrderBy(x => x.Flow.Order).First().Definition.EntityName,
                              History = c.OrderBy(x => x.Flow.Order).First().History,
                              Flow = c.OrderBy(x => x.Flow.Order).First().Flow
                          }
                      )
                      .Where(c => (c.Flow.ApproveType == ApproveType.User && c.Flow.Approver.Equals(username, StringComparison.OrdinalIgnoreCase))
                                    || (c.Flow.ApproveType == ApproveType.Role && roles.Any(r => r.Equals(c.Flow.Approver, StringComparison.OrdinalIgnoreCase))));
    }
}
