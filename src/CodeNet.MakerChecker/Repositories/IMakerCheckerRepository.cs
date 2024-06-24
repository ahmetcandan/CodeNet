using CodeNet.MakerChecker.Models;
using System.Linq.Expressions;

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
    List<TMakerCheckerEntity> FindByStatus(Expression<Func<TMakerCheckerEntity, bool>> predicate, ApproveStatus approveStatus, bool isActive = true);
    Task<List<TMakerCheckerEntity>> FindByStatusAsync(Expression<Func<TMakerCheckerEntity, bool>> predicate, ApproveStatus approveStatus, bool isActive = true, CancellationToken cancellationToken = default);
    TMakerCheckerEntity? GetByReferenceId(Guid referenceId, ApproveStatus approveStatus, bool isActive = true);
    Task<TMakerCheckerEntity?> GetByReferenceIdAsync(Guid referenceId, ApproveStatus approveStatus, bool isActive = true, CancellationToken cancellationToken = default);
}