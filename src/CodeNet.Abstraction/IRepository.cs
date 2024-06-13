using CodeNet.Abstraction.Model;
using System.Linq.Expressions;

namespace CodeNet.Abstraction;

public interface IRepository<TEntity> where TEntity : IEntity
{
    TEntity Get(params object[] keyValues);
    Task<TEntity> GetAsync(params object[] keyValues);
    Task<TEntity> GetAsync(object[] keyValues, CancellationToken cancellationToken);
    Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    TEntity Add(TEntity entity);
    IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    TEntity Update(TEntity entity);

    TEntity Remove(TEntity entity);
    IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities);

    int SaveChanges();
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
