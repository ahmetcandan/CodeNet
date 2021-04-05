using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;

namespace Net5Api.Abstraction
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        public void SetUser(IPrincipal user);
        public IPrincipal GetUser();

        TEntity Get(params object[] keyValues);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);

        TEntity Remove(TEntity entity);
        IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities);
    }
}
