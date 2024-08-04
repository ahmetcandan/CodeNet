﻿using CodeNet.EntityFramework.Models;
using System.Linq.Expressions;

namespace CodeNet.EntityFramework.Repositories;

public interface IBaseRepository<TBaseEntity> : IRepository<TBaseEntity> 
    where TBaseEntity : class, IBaseEntity
{
    List<TBaseEntity> Find(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true);
    Task<List<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive = true, CancellationToken cancellationToken = default);
    TBaseEntity HardDelete(TBaseEntity entity);
    IEnumerable<TBaseEntity> HardDeleteRange(IEnumerable<TBaseEntity> entities);
    TBaseEntity? Get(Expression<Func<TBaseEntity, bool>> predicate, bool isActive);
    Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive);
    Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, CancellationToken cancellationToken);

    PagingList<TBaseEntity> GetPagingList<TKey>(bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count);
    PagingList<TBaseEntity> GetPagingList<TKey>(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count);
    Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count);
    Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count);
    Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAcending, int page, int count, CancellationToken cancellationToken);
    Task<PagingList<TBaseEntity>> GetPagingListAsync<TKey>(Expression<Func<TBaseEntity, bool>> predicate, bool isActive, Expression<Func<TBaseEntity, TKey>> orderBySelector, bool isAscending, int page, int count, CancellationToken cancellationToken);

    IQueryable<TBaseEntity> GetQueryable(Expression<Func<TBaseEntity, bool>> predicate, bool isActive);
}
