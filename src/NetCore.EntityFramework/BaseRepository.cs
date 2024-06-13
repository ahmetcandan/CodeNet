using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System.Linq.Expressions;

namespace NetCore.EntityFramework;

public abstract class BaseRepository<TBaseEntity>(DbContext dbContext) : Repository<TBaseEntity>(dbContext), IBaseRepository<TBaseEntity> 
    where TBaseEntity : class, IBaseEntity
{
    public override TBaseEntity Remove(TBaseEntity entity)
    {
        entity.IsDeleted = true;
        return Update(entity);
    }

    public override IEnumerable<TBaseEntity> RemoveRange(IEnumerable<TBaseEntity> entities)
    {
        foreach (var entity in entities)
            entity.IsDeleted = true;

        return UpdateRange(entities);
    }

    public override Task<List<TBaseEntity>> Find(Expression<Func<TBaseEntity, bool>> predicate)
    {
        return Find(predicate, CancellationToken.None);
    }

    public override Task<List<TBaseEntity>> Find(Expression<Func<TBaseEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return Find(predicate, true, false, cancellationToken);
    }

    public virtual Task<List<TBaseEntity>> Find(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true, bool isDeleted = false, CancellationToken cancellationToken = default)
    {
        return base.Find(AddCondition(c => c.IsActive == isActive && c.IsDeleted == isDeleted, predicate), cancellationToken);
    }

    private static Expression<Func<TBaseEntity, bool>> AddCondition(Expression<Func<TBaseEntity, bool>> originalPredicate, Expression<Func<TBaseEntity, bool>> additionalCondition)
    {
        var parameter = Expression.Parameter(typeof(TBaseEntity));
        var body = Expression.AndAlso(
            Expression.Invoke(originalPredicate, parameter),
            Expression.Invoke(additionalCondition, parameter)
        );

        return Expression.Lambda<Func<TBaseEntity, bool>>(body, parameter);
    }
}
