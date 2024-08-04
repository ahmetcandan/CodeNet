using CodeNet.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Threading;

namespace CodeNet.EntityFramework.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _entities;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = _dbContext.Set<TEntity>();
    }

    #region Add
    public virtual TEntity Add(TEntity entity)
    {
        return _entities.Add(entity).Entity;
    }

    public virtual Task<TEntity> AddAsync(TEntity entity)
    {
        return AddAsync(entity, CancellationToken.None);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return (await _entities.AddAsync(entity, cancellationToken)).Entity;
    }

    public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
    {
        _entities.AddRange(entities);
        return entities;
    }

    public virtual Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        return AddRangeAsync(entities, CancellationToken.None);
    }

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
    public virtual PagingList<TEntity> GetPagingList(int page, int count)
    {
        return GetPagingList(c => true, page, count);
    }

    public virtual PagingList<TEntity> GetPagingList<TKey>(Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
    {
        return GetPagingList(c => true, orderBySelector, isAcending, page, count);
    }

    public virtual PagingList<TEntity> GetPagingList(Expression<Func<TEntity, bool>> predicate, int page, int count)
    {
        int totalCount = _entities.Count(predicate);
        List<TEntity> list = page < 1 || count < 1
            ? throw new ArgumentException("Page or count cannot be less than 1")
            : ([.. _entities.Where(predicate).Skip((page - 1) * count).Take(count)]);

        return new PagingList<TEntity>
        {
            List = list,
            PageCount = count,
            PageNumber = page,
            TotalCount = totalCount
        };
    }

    public virtual PagingList<TEntity> GetPagingList<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
    {
        int totalCount = _entities.Count(predicate);
        List<TEntity> list = page < 1 || count < 1
            ? throw new ArgumentException("Page or count cannot be less than 1")
            : (isAcending
                ? [.. _entities.OrderBy(orderBySelector).Where(predicate).Skip((page - 1) * count).Take(count)]
                : [.. _entities.OrderByDescending(orderBySelector).Where(predicate).Skip((page - 1) * count).Take(count)]);

        return new PagingList<TEntity>
        {
            List = list,
            PageCount = count,
            PageNumber = page,
            TotalCount = totalCount
        };
    }

    public virtual Task<PagingList<TEntity>> GetPagingListAsync(int page, int count)
    {
        return GetPagingListAsync(page, count, CancellationToken.None);
    }

    public virtual Task<PagingList<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
    {
        return GetPagingListAsync(orderBySelector, isAcending, page, count, CancellationToken.None);
    }

    public virtual Task<PagingList<TEntity>> GetPagingListAsync(Expression<Func<TEntity, bool>> predicate, int page, int count)
    {
        return GetPagingListAsync(predicate, page, count, CancellationToken.None);
    }

    public virtual Task<PagingList<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
    {
        return GetPagingListAsync(predicate, orderBySelector, isAcending, page, count, CancellationToken.None);
    }

    public virtual Task<PagingList<TEntity>> GetPagingListAsync(int page, int count, CancellationToken cancellationToken)
    {
        return GetPagingListAsync(c => true, page, count, cancellationToken);
    }

    public virtual Task<PagingList<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, TKey>> orderBySelector, bool isAcending, int page, int count, CancellationToken cancellationToken)
    {
        return GetPagingListAsync(c => true, orderBySelector, isAcending, page, count, cancellationToken);
    }

    public virtual async Task<PagingList<TEntity>> GetPagingListAsync(Expression<Func<TEntity, bool>> predicate, int page, int count, CancellationToken cancellationToken)
    {
        int totalCount = await _entities.CountAsync(predicate, cancellationToken);
        List<TEntity> list = page < 1 || count < 1
            ? throw new ArgumentException("Page or count cannot be less than 1")
            : await _entities.Where(predicate).Skip((page - 1) * count).Take(count).ToListAsync(cancellationToken);

        return new PagingList<TEntity>
        {
            List = list,
            PageCount = count,
            PageNumber = page,
            TotalCount = totalCount
        };
    }

    public virtual async Task<PagingList<TEntity>> GetPagingListAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBySelector, bool isAscending, int page, int count, CancellationToken cancellationToken)
    {
        int totalCount = await _entities.CountAsync(predicate, cancellationToken);
        List<TEntity> list = page < 1 || count < 1
            ? throw new ArgumentException("Page or count cannot be less than 1")
            : (isAscending
                ? await _entities.OrderBy(orderBySelector).Where(predicate).Skip((page - 1) * count).Take(count).ToListAsync(cancellationToken)
                : await _entities.OrderByDescending(orderBySelector).Where(predicate).Skip((page - 1) * count).Take(count).ToListAsync(cancellationToken));

        return new PagingList<TEntity>
        {
            List = list,
            PageCount = count,
            PageNumber = page,
            TotalCount = totalCount
        };
    }
    #endregion

    public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.Where(predicate);
    }

    #region Find
    public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return [.. _entities.Where(predicate)];
    }

    public virtual Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return FindAsync(predicate, CancellationToken.None);
    }

    public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _entities.Where(predicate).ToListAsync(cancellationToken);
    }
    #endregion

    #region Get
    public virtual TEntity? Get(params object[] keyValues)
    {
        return _entities.Find(keyValues);
    }

    public virtual TEntity? Get(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.FirstOrDefault(predicate);
    }

    public virtual Task<TEntity?> GetAsync(params object[] keyValues)
    {
        return GetAsync(keyValues, CancellationToken.None);
    }

    public virtual async Task<TEntity?> GetAsync(object[] keyValues, CancellationToken cancellationToken)
    {
        return await _entities.FindAsync(keyValues, cancellationToken);
    }

    public virtual Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return GetAsync(predicate, CancellationToken.None);
    }

    public virtual Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return _entities.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }
    #endregion

    #region Remove
    public virtual TEntity Remove(TEntity entity)
    {
        return _entities.Remove(entity).Entity;
    }

    public virtual IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
    {
        _entities.RemoveRange(entities);
        return entities;
    }
    #endregion

    #region SaveChanges
    public virtual int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    public virtual Task<int> SaveChangesAsync()
    {
        return SaveChangesAsync(CancellationToken.None);
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion
}
