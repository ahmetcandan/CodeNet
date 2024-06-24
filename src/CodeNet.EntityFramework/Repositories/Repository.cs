﻿using CodeNet.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;

namespace CodeNet.EntityFramework.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
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
        _dbContext.Entry(entities).State = EntityState.Modified;
        return entities;
    }

    public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.Where(predicate).ToList();
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

    public virtual Task<TEntity?> GetAsync(params object[] keyValues)
    {
        return GetAsync(keyValues, CancellationToken.None);
    }

    public virtual async Task<TEntity?> GetAsync(object[] keyValues, CancellationToken cancellationToken)
    {
        return await _entities.FindAsync(keyValues, cancellationToken);
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
