using CodeNet.EntityFramework.Models;
using System.Linq.Expressions;

namespace CodeNet.EntityFramework.Repositories;

public interface IRepository<TEntity> 
    where TEntity : class
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

    List<TEntity> GetPagingList(int page, int count);
    List<TEntity> GetPagingList<TKey>(Expression<Func<TEntity, TKey>> orderBy, int page, int count);
    List<TEntity> GetPagingList(Expression<Func<TEntity, bool>> predicate, int page, int count);
    List<TEntity> GetPagingList<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int page, int count);
    Task<List<TEntity>> GetPagingListAsync(int page, int count);
    Task<List<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, int page, int count);
    Task<List<TEntity>> GetPagingListAsync(Expression<Func<TEntity, bool>> predicate, int page, int count);
    Task<List<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int page, int count);
    Task<List<TEntity>> GetPagingListAsync(int page, int count, CancellationToken cancellationToken);
    Task<List<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, int page, int count, CancellationToken cancellationToken);
    Task<List<TEntity>> GetPagingListAsync(Expression<Func<TEntity, bool>> predicate, int page, int count, CancellationToken cancellationToken);
    Task<List<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, int page, int count, CancellationToken cancellationToken);

    #region Queryable
    IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate);
    #endregion

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
