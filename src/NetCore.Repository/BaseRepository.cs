using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _entities;
        private readonly IIdentityContext _identityContext;

        public BaseRepository(DbContext dbContext, IIdentityContext identityContext)
        {
            _dbContext = dbContext;
            _identityContext = identityContext;
            _entities = _dbContext.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            if (typeof(TEntity).GetInterfaces().Any(c => c == typeof(ITracingEntity)))
            {
                var tEntity = (ITracingEntity)entity;
                tEntity.CreatedDate = DateTime.Now;
                tEntity.CreatedUser = _identityContext.GetUserName();
            }
            return _dbContext.Set<TEntity>().Add(entity).Entity;
        }

        public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return AddAsync(entity, cancellationToken);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (typeof(TEntity).GetInterfaces().Any(c => c == typeof(ITracingEntity)))
            {
                var tEntity = (ITracingEntity)entity;
                tEntity.CreatedDate = DateTime.Now;
                tEntity.CreatedUser = _identityContext.GetUserName();
            }
            return (await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            if (typeof(TEntity).GetInterfaces().Any(c => c == typeof(ITracingEntity)))
            {
                foreach (var entity in entities)
                {
                    var tEntity = (ITracingEntity)entity;
                    tEntity.CreatedDate = DateTime.Now;
                    tEntity.CreatedUser = _identityContext.GetUserName();
                }
            }
            _dbContext.Set<TEntity>().AddRange(entities);
            return entities;
        }

        public Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return AddRangeAsync(entities, CancellationToken.None);
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            if (typeof(TEntity).GetInterfaces().Any(c => c == typeof(ITracingEntity)))
            {
                foreach (var entity in entities)
                {
                    var tEntity = (ITracingEntity)entity;
                    tEntity.CreatedDate = DateTime.Now;
                    tEntity.CreatedUser = _identityContext.GetUserName();
                }
            }
            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            if (typeof(TEntity).GetInterfaces().Any(c => c == typeof(ITracingEntity)))
            {
                var tEntity = (ITracingEntity)entity;
                tEntity.ModifiedDate = DateTime.Now;
                tEntity.ModifiedUser = _identityContext.GetUserName();
            }
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        public TEntity Get(params object[] keyValues)
        {
            return _dbContext.Set<TEntity>().Find(keyValues);
        }

        public Task<TEntity> GetAsync(params object[] keyValues)
        {
            return GetAsync(keyValues, CancellationToken.None);
        }

        public async Task<TEntity> GetAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TEntity>().FindAsync(keyValues, cancellationToken);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public TEntity Remove(TEntity entity)
        {
            return _dbContext.Set<TEntity>().Remove(entity).Entity;
        }

        public IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            return entities;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
