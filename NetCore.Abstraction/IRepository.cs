using NetCore.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Abstraction
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        TEntity Get(params object[] keyValues);
        Task<TEntity> GetAsync(params object[] keyValues);
        Task<TEntity> GetAsync(object[] keyValues, CancellationToken cancellationToken);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

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
}
