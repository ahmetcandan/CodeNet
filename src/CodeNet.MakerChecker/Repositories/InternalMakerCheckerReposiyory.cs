using CodeNet.Core.Context;
using CodeNet.MakerChecker.DbContext;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Repositories;

internal class InternalMakerCheckerReposiyory<TMakerCheckerEntity>(MakerCheckerDbContext dbContext, ICodeNetContext codeNetContext) : MakerCheckerRepository<TMakerCheckerEntity>(dbContext, codeNetContext)
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    public List<MakerCheckerFlowHistory> GetMakerCheckerFlowHistoryList(Guid referenceId) => [.. GetMakerCheckerFlowHistoryListQueryable(referenceId)];

    public Task<List<MakerCheckerFlowHistory>> GetMakerCheckerFlowHistoryListAsync(Guid referenceId, CancellationToken cancellationToken)
        => GetMakerCheckerFlowHistoryListQueryable(referenceId).ToListAsync(cancellationToken);
}
