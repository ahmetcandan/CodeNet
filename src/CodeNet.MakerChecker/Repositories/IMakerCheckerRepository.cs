using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Repositories;

public interface IMakerCheckerRepository<TMakerCheckerEntity>
    where TMakerCheckerEntity : class, IMakerCheckerEntity
{
    TMakerCheckerEntity Add(TMakerCheckerEntity entity);
    Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity);
    Task<TMakerCheckerEntity> AddAsync(TMakerCheckerEntity entity, CancellationToken cancellationToken);
    TMakerCheckerEntity Update(TMakerCheckerEntity entity);
    List<MakerCheckerFlow> GetMakerCheckerFlows();
    Task<List<MakerCheckerFlow>> GetAsyncMakerCheckerFlows(CancellationToken cancellationToken);
}