using CodeNet.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CodeNet.EntityFramework.Repositories;

public class BaseRepository<TBaseEntity>(DbContext dbContext) : Repository<TBaseEntity>(dbContext), IBaseRepository<TBaseEntity>
    where TBaseEntity : class, IBaseEntity
{
    private readonly bool _isSoftDelete = typeof(TBaseEntity).GetInterface(nameof(ISoftDelete)) is not null;

    #region Remove
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
    #endregion

    #region Get
    public override TBaseEntity? Get(Expression<Func<TBaseEntity, bool>> predicate)
    {
        return Get(predicate: predicate, isActive: true);
    }

    public virtual TBaseEntity? Get(Expression<Func<TBaseEntity, bool>> predicate, bool isActive)
    {
        return base.Get(AddCondition(c => c.IsActive == isActive && !c.IsDeleted, predicate));
    }

    public override Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate)
    {
        return GetAsync(predicate, true);
    }

    public virtual Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive)
    {
        return GetAsync(predicate, isActive, CancellationToken.None);
    }

    public override Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return GetAsync(predicate, true, cancellationToken);
    }

    public virtual Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, CancellationToken cancellationToken)
    {
        return base.GetAsync(AddCondition(c => c.IsActive == isActive && !c.IsDeleted, predicate), cancellationToken);
    }
    #endregion

    #region Find
    public override List<TBaseEntity> Find(Expression<Func<TBaseEntity, bool>> predicate)
    {
        return Find(predicate, true);
    }

    public virtual List<TBaseEntity> Find(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true)
    {
        return base.Find(AddCondition(c => c.IsActive == isActive && !c.IsDeleted, predicate));
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
        return base.FindAsync(AddCondition(c => c.IsActive == isActive && !c.IsDeleted, predicate), cancellationToken);
    }
    #endregion

    #region PagingList
    public override List<TBaseEntity> GetPagingList(int page, int count)
    {
        return GetPagingList(true, page, count);
    }

    public virtual List<TBaseEntity> GetPagingList(bool isActive, int page, int count)
    {
        return base.GetPagingList(c => c.IsActive == isActive && !c.IsDeleted, page, count);
    }

    public override List<TBaseEntity> GetPagingList(Expression<Func<TBaseEntity, bool>> predicate, int page, int count)
    {
        return GetPagingList(predicate, true, page, count);
    }

    public virtual List<TBaseEntity> GetPagingList(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, int page, int count)
    {
        return base.GetPagingList(AddCondition(predicate, c => c.IsActive == isActive && !c.IsDeleted), page, count);
    }

    public override Task<List<TBaseEntity>> GetPagingListAsync(int page, int count)
    {
        return GetPagingListAsync(true, page, count);
    }

    public virtual Task<List<TBaseEntity>> GetPagingListAsync(bool isActive, int page, int count)
    {
        return GetPagingListAsync(isActive, page, count, CancellationToken.None);
    }

    public override Task<List<TBaseEntity>> GetPagingListAsync(Expression<Func<TBaseEntity, bool>> predicate, int page, int count)
    {
        return GetPagingListAsync(predicate, page, count, CancellationToken.None);
    }

    public virtual Task<List<TBaseEntity>> GetPagingListAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, int page, int count)
    {
        return GetPagingListAsync(predicate, isActive, page, count, CancellationToken.None);
    }

    public override Task<List<TBaseEntity>> GetPagingListAsync(int page, int count, CancellationToken cancellationToken)
    {
        return GetPagingListAsync(true, page, count, cancellationToken);
    }

    public virtual Task<List<TBaseEntity>> GetPagingListAsync(bool isActive, int page, int count, CancellationToken cancellationToken)
    {
        return base.GetPagingListAsync(c => c.IsActive == isActive && !c.IsDeleted, page, count, cancellationToken);
    }

    public override Task<List<TBaseEntity>> GetPagingListAsync(Expression<Func<TBaseEntity, bool>> predicate, int page, int count, CancellationToken cancellationToken)
    {
        return GetPagingListAsync(predicate, true, page, count, cancellationToken);
    }

    public virtual Task<List<TBaseEntity>> GetPagingListAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, int page, int count, CancellationToken cancellationToken)
    {
        return base.GetPagingListAsync(AddCondition(predicate, c => c.IsActive == isActive && c.IsDeleted), page, count, cancellationToken);
    }

    public override Task<List<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, bool>> predicate, Expression<Func<TBaseEntity, TKey>> orderBy, int page, int count, CancellationToken cancellationToken)
    {
        return base.GetPagingListAsync(AddCondition(predicate, c => c.IsActive && !c.IsDeleted), orderBy, page, count, cancellationToken);
    }

    public override Task<List<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, TKey>> orderBy, int page, int count, CancellationToken cancellationToken)
    {
        return GetPagingListAsync(c => c.IsActive && !c.IsDeleted, orderBy, page, count, cancellationToken);
    }

    public override List<TBaseEntity> GetPagingList<TKey>(Expression<Func<TBaseEntity, bool>> predicate, Expression<Func<TBaseEntity, TKey>> orderBy, int page, int count)
    {
        return base.GetPagingList(AddCondition(predicate, c => c.IsActive && !c.IsDeleted), orderBy, page, count);
    }

    public override List<TBaseEntity> GetPagingList<TKey>(Expression<Func<TBaseEntity, TKey>> orderBy, int page, int count)
    {
        return base.GetPagingList(c => c.IsActive && !c.IsDeleted, orderBy, page, count);
    }

    public override Task<List<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, bool>> predicate, Expression<Func<TBaseEntity, TKey>> orderBy, int page, int count)
    {
        return base.GetPagingListAsync(AddCondition(predicate, c => c.IsActive && !c.IsDeleted), orderBy, page, count);
    }

    public override Task<List<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, TKey>> orderBy, int page, int count)
    {
        return base.GetPagingListAsync(c => c.IsActive && !c.IsDeleted, orderBy, page, count);
    }
    #endregion

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
