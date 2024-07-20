using CodeNet.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodeNet.EntityFramework.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> 
    where TEntity : class, IEntity
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _entities;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = _dbContext.Set<TEntity>();
    }

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

    public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.Where(predicate).ToList();
    }

    public virtual List<TEntity> GetPagingList(int page, int count)
    {
        if (page < 1 || count < 1)
            throw new ArgumentException("Page or count cannot be less than 1");

        return [.. _entities.Skip((page - 1) * count).Take(count)];
    }

    public virtual Task<List<TEntity>> GetPagingListAsync(int page, int count)
    {
        return GetPagingListAsync(page, count, CancellationToken.None);
    }

    public async virtual Task<List<TEntity>> GetPagingListAsync(int page, int count, CancellationToken cancellationToken)
    {
        if (page < 1 || count < 1)
            throw new ArgumentException("Page or count cannot be less than 1");

        return await _entities.Skip((page - 1) * count).Take(count).ToListAsync(cancellationToken);
    }

    public virtual Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return FindAsync(predicate, CancellationToken.None);
    }

    public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _entities.Where(predicate).ToListAsync(cancellationToken);
    }

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

    public virtual TEntity Remove(TEntity entity)
    {
        return _entities.Remove(entity).Entity;
    }

    public virtual IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
    {
        _entities.RemoveRange(entities);
        return entities;
    }

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
}
