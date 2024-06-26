using CodeNet.EntityFramework.Repositories;
using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

public interface IMakerCheckerRepository<TMakerCheckerEntity> : ITracingRepository<TMakerCheckerEntity>
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    TMakerCheckerEntity? Approve(MakerCheckerDraftEntity draft, string description);
    Task<TMakerCheckerEntity?> ApproveAsync(MakerCheckerDraftEntity draft, string description, CancellationToken cancellationToken = default);
    void Reject(MakerCheckerDraftEntity draft, string description);
    Task RejectAsync(MakerCheckerDraftEntity draft, string description, CancellationToken cancellationToken = default);
    MakerCheckerDraftEntity? GetDraft(Guid referenceId);
    Task<MakerCheckerDraftEntity?> GetDraftAsync(Guid referenceId, CancellationToken cancellationToken = default);
}