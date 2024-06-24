using CodeNet.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodeNet.EntityFramework.Repositories;

public abstract class BaseRepository<TBaseEntity>(DbContext dbContext) : Repository<TBaseEntity>(dbContext), IBaseRepository<TBaseEntity>
    where TBaseEntity : class, IBaseEntity
{
    private readonly bool _isSoftDelete = typeof(TBaseEntity).GetInterface(nameof(ISoftDelete)) is not null;

    public override TBaseEntity Remove(TBaseEntity entity)
    {
        if (_isSoftDelete)
        {
            entity.IsDeleted = true;
            return Update(entity);
        }

        return base.Remove(entity);
    }

    public virtual TBaseEntity HardDelete(TBaseEntity entity)
    {
        return base.Remove(entity);
    }

    public override IEnumerable<TBaseEntity> RemoveRange(IEnumerable<TBaseEntity> entities)
    {
        if (_isSoftDelete)
        {
            foreach (var entity in entities)
                entity.IsDeleted = true;
            return UpdateRange(entities);
        }

        return base.RemoveRange(entities);
    }

    public virtual IEnumerable<TBaseEntity> HardDeleteRange(IEnumerable<TBaseEntity> entities)
    {
        return base.RemoveRange(entities);
    }

    public override List<TBaseEntity> Find(Expression<Func<TBaseEntity, bool>> predicate)
    {
        return Find(predicate, true);
    }

    public virtual List<TBaseEntity> Find(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true)
    {
        return base.Find(AddCondition(c => c.IsActive == isActive, predicate));
    }

    public override Task<List<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate)
    {
        return FindAsync(predicate, CancellationToken.None);
    }

    public override Task<List<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return FindAsync(predicate, true, cancellationToken);
    }

    public virtual Task<List<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true, CancellationToken cancellationToken = default)
    {
        return base.FindAsync(AddCondition(c => c.IsActive == isActive, predicate), cancellationToken);
    }

    protected static Expression<Func<TBaseEntity, bool>> AddCondition(Expression<Func<TBaseEntity, bool>> originalPredicate, Expression<Func<TBaseEntity, bool>> additionalCondition)
    {
        var parameter = Expression.Parameter(typeof(TBaseEntity));
        var body = Expression.AndAlso(
            Expression.Invoke(originalPredicate, parameter),
            Expression.Invoke(additionalCondition, parameter)
        );

        return Expression.Lambda<Func<TBaseEntity, bool>>(body, parameter);
    }
}
