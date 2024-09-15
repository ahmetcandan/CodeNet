using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

public interface IMakerCheckerRepository<TMakerCheckerEntity> : ITracingRepository<TMakerCheckerEntity>
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    TMakerCheckerEntity? GetByReferenceId(Guid referenceId);
    Task<TMakerCheckerEntity?> GetByReferenceIdAsync(Guid referenceId, CancellationToken cancellationToken = default);
}