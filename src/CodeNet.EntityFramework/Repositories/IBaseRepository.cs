using CodeNet.EntityFramework.Models;
using System.Linq.Expressions;

namespace CodeNet.EntityFramework.Repositories;

public interface IBaseRepository<TBaseEntity> : IRepository<TBaseEntity> where TBaseEntity : class, IBaseEntity
{
    List<TBaseEntity> Find(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true);
    Task<List<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true, CancellationToken cancellationToken = default);
    TBaseEntity HardDelete(TBaseEntity entity);
    IEnumerable<TBaseEntity> HardDeleteRange(IEnumerable<TBaseEntity> entities);
}
