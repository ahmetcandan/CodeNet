using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

public interface IMakerCheckerRepository<TMakerCheckerEntity>
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    TMakerCheckerEntity Add(TMakerCheckerEntity entity);
    Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity);
    Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken);
    TMakerCheckerEntity Update(TMakerCheckerEntity entity);
    void Approve(TMakerCheckerEntity entity);
    Task ApproveAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken = default);
    void Reject(TMakerCheckerEntity entity);
    Task RejectAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken = default);
}