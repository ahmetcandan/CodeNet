using CodeNet.EntityFramework.Models;
using System.Linq.Expressions;

namespace CodeNet.EntityFramework.Repositories;

public interface IRepository<TEntity> 
    where TEntity : class, IEntity
{
    TEntity? Get(params object[] keyValues);
    TEntity? Get(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetAsync(params object[] keyValues);
    Task<TEntity?> GetAsync(object[] keyValues, CancellationToken cancellationToken);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    List<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

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
