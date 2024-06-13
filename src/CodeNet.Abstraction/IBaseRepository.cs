using CodeNet.Abstraction.Model;
using System.Linq.Expressions;

namespace CodeNet.Abstraction;

public interface IBaseRepository<TBaseEntity> : IRepository<TBaseEntity> where TBaseEntity : class, IBaseEntity
{
    Task<List<TBaseEntity>> Find(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true, bool isDeleted = false, CancellationToken cancellationToken = default);
}
