using NetCore.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Abstraction
{
    public interface IBaseRepository<TBaseEntity> : IRepository<TBaseEntity> where TBaseEntity : class, IBaseEntity
    {
        Task<List<TBaseEntity>> Find(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true, bool isDeleted = false, CancellationToken cancellationToken = default);
    }
}
