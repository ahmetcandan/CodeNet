using CodeNet.Core;
using CodeNet.MakerChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.MakerChecker.Repositories;

internal class InternalMakerCheckerReposiyory<TMakerCheckerEntity>(MakerCheckerDbContext dbContext, ICodeNetContext codeNetContext) : MakerCheckerRepository<TMakerCheckerEntity>(dbContext, codeNetContext)
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    public List<MakerCheckerFlowHistory> GetMakerCheckerFlowHistoryList(Guid referenceId)
    {
        return [.. GetMakerCheckerFlowHistoryListQueryable(referenceId)];
    }

    public Task<List<MakerCheckerFlowHistory>> GetMakerCheckerFlowHistoryListAsync(Guid referenceId, CancellationToken cancellationToken)
    {
        return GetMakerCheckerFlowHistoryListQueryable(referenceId).ToListAsync(cancellationToken);
    }
}
