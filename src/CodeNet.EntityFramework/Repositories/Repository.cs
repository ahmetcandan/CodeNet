using CodeNet.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodeNet.EntityFramework.Repositories;

public class Repository<TEntity>(DbContext dbContext) : IRepository<TEntity>
    where TEntity : class
{
    protected readonly DbContext _dbContext = dbContext;
    protected readonly DbSet<TEntity> _entities = dbContext.Set<TEntity>();

    #region Add
    public virtual TEntity Add(TEntity entity) => _entities.Add(entity).Entity;

    public virtual Task<TEntity> AddAsync(TEntity entity) => AddAsync(entity, CancellationToken.None);

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken) => (await _entities.AddAsync(entity, cancellationToken)).Entity;

    public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
    {
        _entities.AddRange(entities);
        return entities;
    }

    public virtual Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities) => AddRangeAsync(entities, CancellationToken.None);

    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await _entities.AddRangeAsync(entities, cancellationToken);
        return entities;
    }
    #endregion

    #region Update
    public virtual TEntity Update(TEntity entity)
    {
        _entities.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public virtual IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
    {
        _entities.AttachRange(entities);
        foreach (var entity in entities)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entities;
    }
    #endregion

    #region Paging List
    public virtual PagingList<TEntity> GetPagingList<TKey>(Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
        => GetPagingList(c => true, orderBySelector, isAcending, page, count);

    public virtual PagingList<TEntity> GetPagingList<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
    {
        List<TEntity> list = page < 1 || count < 1
            ? throw new ArgumentException("Page or count cannot be less than 1")
            : (isAcending
                ? [.. _entities.OrderBy(orderBySelector).Where(predicate).Skip((page - 1) * count).Take(count)]
                : [.. _entities.OrderByDescending(orderBySelector).Where(predicate).Skip((page - 1) * count).Take(count)]);

        return new()
        {
            List = list,
            PageCount = count,
            PageNumber = page,
            TotalCount = _entities.Count(predicate)
        };
    }


    public virtual Task<PagingList<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
        => GetPagingListAsync(orderBySelector, isAcending, page, count, CancellationToken.None);

    public virtual Task<PagingList<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
        => GetPagingListAsync(predicate, orderBySelector, isAcending, page, count, CancellationToken.None);

    public virtual Task<PagingList<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count, CancellationToken cancellationToken)
        => GetPagingListAsync(c => true, orderBySelector, isAcending, page, count, cancellationToken);

    public virtual async Task<PagingList<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBySelector, bool isAscending, int page, int count, CancellationToken cancellationToken)
    {
        int totalCount = await _entities.CountAsync(predicate, cancellationToken);
        List<TEntity> list = page < 1 || count < 1
            ? throw new ArgumentException("Page or count cannot be less than 1")
            : (isAscending
                ? await _entities.OrderBy(orderBySelector).Where(predicate).Skip((page - 1) * count).Take(count).ToListAsync(cancellationToken)
                : await _entities.OrderByDescending(orderBySelector).Where(predicate).Skip((page - 1) * count).Take(count).ToListAsync(cancellationToken));

        return new()
        {
            List = list,
            PageCount = count,
            PageNumber = page,
            TotalCount = totalCount
        };
    }
    #endregion

    public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate) => _entities.Where(predicate);

    #region Find
    public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate) => [.. _entities.Where(predicate)];

    public virtual Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) => FindAsync(predicate, CancellationToken.None);

    public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
         => await _entities.Where(predicate).ToListAsync(cancellationToken);
    #endregion

    #region Get
    public virtual TEntity? Get(params object[] keyValues) => _entities.Find(keyValues);

    public virtual TEntity? Get(Expression<Func<TEntity, bool>> predicate) => _entities.FirstOrDefault(predicate);

    public virtual Task<TEntity?> GetAsync(params object[] keyValues) => GetAsync(keyValues, CancellationToken.None);

    public virtual async Task<TEntity?> GetAsync(object[] keyValues, CancellationToken cancellationToken)
        => await _entities.FindAsync(keyValues, cancellationToken);

    public virtual Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate) => GetAsync(predicate, CancellationToken.None);

    public virtual Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => _entities.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    #endregion

    #region Remove
    public virtual TEntity Remove(TEntity entity) => _entities.Remove(entity).Entity;

    public virtual IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
    {
        _entities.RemoveRange(entities);
        return entities;
    }
    #endregion

    #region SaveChanges
    public virtual int SaveChanges() => _dbContext.SaveChanges();

    public virtual Task<int> SaveChangesAsync() => SaveChangesAsync(CancellationToken.None);

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => await _dbContext.SaveChangesAsync(cancellationToken);
    #endregion
}
