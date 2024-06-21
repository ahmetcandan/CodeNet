using CodeNet.Core;
using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

public class MakerCheckerHistoryRepository(MakerCheckerDbContext makerCheckerDbContext, IIdentityContext identityContext) : TracingRepository<MakerCheckerHistory>(makerCheckerDbContext, identityContext), IMakerCheckerHistoryRepository
{
    public override MakerCheckerHistory Add(MakerCheckerHistory entity)
    {
        return base.Add(entity);
    }

    public override Task<MakerCheckerHistory> AddAsync(MakerCheckerHistory entity)
    {
        return AddAsync(entity, CancellationToken.None);
    }

    public override Task<MakerCheckerHistory> AddAsync(MakerCheckerHistory entity, CancellationToken cancellationToken)
    {
        return base.AddAsync(entity, cancellationToken);
    }
}
