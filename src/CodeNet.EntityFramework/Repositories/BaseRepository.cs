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

    public virtual TBaseEntity HardDelete(TBaseEntity entity) => base.Remove(entity);

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

    public virtual IEnumerable<TBaseEntity> HardDeleteRange(IEnumerable<TBaseEntity> entities) => base.RemoveRange(entities);
    #endregion

    #region Get
    public override TBaseEntity? Get(Expression<Func<TBaseEntity, bool>> predicate)
    {
        return Get(predicate: predicate, isActive: true);
    }

    public virtual TBaseEntity? Get(Expression<Func<TBaseEntity, bool>> predicate, bool isActive) => base.Get(AddCondition(c => c.IsActive == isActive && !c.IsDeleted, predicate));

    public override Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate) => GetAsync(predicate, true);

    public virtual Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive) => GetAsync(predicate, isActive, CancellationToken.None);

    public override Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate, CancellationToken cancellationToken) => GetAsync(predicate, true, cancellationToken);

    public virtual Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, CancellationToken cancellationToken) 
        => base.GetAsync(AddCondition(c => c.IsActive == isActive && !c.IsDeleted, predicate), cancellationToken);
    #endregion

    #region Find
    public override List<TBaseEntity> Find(Expression<Func<TBaseEntity, bool>> predicate) => Find(predicate, true);

    public virtual List<TBaseEntity> Find(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true) => base.Find(AddCondition(c => c.IsActive == isActive && !c.IsDeleted, predicate));

    public override Task<List<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate) => FindAsync(predicate, CancellationToken.None);

    public override Task<List<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate, CancellationToken cancellationToken) => FindAsync(predicate, true, cancellationToken);

    public virtual Task<List<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true, CancellationToken cancellationToken = default) 
        => base.FindAsync(AddCondition(c => c.IsActive == isActive && !c.IsDeleted, predicate), cancellationToken);
    #endregion

    #region PagingList

    public override PagingList<TBaseEntity> GetPagingList<TKey>(Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count) 
        => GetPagingList(true, orderBySelector, isAcending, page, count);

    public virtual PagingList<TBaseEntity> GetPagingList<TKey>(bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count) 
        => base.GetPagingList(c => c.IsActive == isActive && !c.IsDeleted, orderBySelector, isAcending, page, count);

    public override PagingList<TBaseEntity> GetPagingList<TKey>(Expression<Func<TBaseEntity, bool>> predicate, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count) 
        => GetPagingList(predicate, true, orderBySelector, isAcending, page, count);

    public virtual PagingList<TBaseEntity> GetPagingList<TKey>(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count) => base.GetPagingList(AddCondition(predicate, c => c.IsActive == isActive && !c.IsDeleted), orderBySelector, isAcending, page, count);

    public override Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count) 
        => GetPagingListAsync(true, orderBySelector, isAcending, page, count);

    public virtual Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
        => base.GetPagingListAsync(c => c.IsActive == isActive && !c.IsDeleted, orderBySelector, isAcending, page, count);

    public override Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, bool>> predicate, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
        => GetPagingListAsync(predicate, true, orderBySelector, isAcending, page, count);

    public virtual Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count)
        => base.GetPagingListAsync(AddCondition(predicate, c => c.IsActive == isActive && !c.IsDeleted), orderBySelector, isAcending, page, count);

    public override Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count, CancellationToken cancellationToken)
        => GetPagingListAsync(true, orderBySelector, isAcending, page, count, cancellationToken);

    public virtual Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count, CancellationToken cancellationToken)
        => base.GetPagingListAsync(c => c.IsActive == isActive && !c.IsDeleted, orderBySelector, isAcending, page, count, cancellationToken);

    public override Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, bool>> predicate, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAscending, int page, int count, CancellationToken cancellationToken)
        => GetPagingListAsync(predicate, true, orderBySelector, isAscending, page, count, cancellationToken);

    public virtual Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAscending, int page, int count, CancellationToken cancellationToken)
        => base.GetPagingListAsync(AddCondition(predicate, c => c.IsActive == isActive && !c.IsDeleted), orderBySelector, isAscending, page, count, cancellationToken);
    #endregion

    #region Queryable
    public override IQueryable<TBaseEntity> GetQueryable(Expression<Func<TBaseEntity, bool>> predicate) => GetQueryable(predicate, true);

    public virtual IQueryable<TBaseEntity> GetQueryable(Expression<Func<TBaseEntity, bool>> predicate, bool isActive)
         => base.GetQueryable(AddCondition(predicate, c => c.IsActive == isActive && !c.IsDeleted));
    #endregion

    protected static Expression<Func<TBaseEntity, bool>> AddCondition(Expression<Func<TBaseEntity, bool>> originalPredicate, Expression<Func<TBaseEntity, bool>> additionalCondition)
    {
        var parameter = Expression.Parameter(typeof(TBaseEntity));

        return Expression.Lambda<Func<TBaseEntity, bool>>(Expression.AndAlso(
            Expression.Invoke(originalPredicate, parameter),
            Expression.Invoke(additionalCondition, parameter)
        ), parameter);
    }
}
