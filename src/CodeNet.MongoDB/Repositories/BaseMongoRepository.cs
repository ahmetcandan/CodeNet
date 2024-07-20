﻿using MongoDB.Driver;
using System.Linq.Expressions;

namespace CodeNet.MongoDB.Repositories;

/// <summary>
/// MongoDB Repository
/// </summary>
/// <typeparam name="TModel"></typeparam>
/// <remarks>
/// MongoDB Repository
/// </remarks>
/// <param name="options"></param>
public class BaseMongoRepository<TModel>(MongoDBContext dbContext) : IMongoDBRepository<TModel> where TModel : class
{
    private readonly IMongoCollection<TModel> _mongoCollection = dbContext.Set<TModel>();

    /// <summary>
    /// Get List
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual List<TModel> GetList(Expression<Func<TModel, bool>> filter)
    {
        return _mongoCollection.Find(filter).ToList();
    }

    /// <summary>
    /// Get By ID
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual TModel GetById(Expression<Func<TModel, bool>> filter)
    {
        return _mongoCollection.Find(filter).FirstOrDefault();
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual TModel Create(TModel model)
    {
        _mongoCollection.InsertOne(model);
        return model;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="model"></param>
    public virtual void Update(Expression<Func<TModel, bool>> filter, TModel model)
    {
        _mongoCollection.ReplaceOne(filter, model);
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="filter"></param>
    public virtual void Delete(Expression<Func<TModel, bool>> filter)
    {
        _mongoCollection.DeleteOne(filter);
    }

    /// <summary>
    /// Exists
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual bool Exists(Expression<Func<TModel, bool>> filter)
    {
        return _mongoCollection.CountDocuments(filter) > 0;
    }

    /// <summary>
    /// Count
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual long Count(Expression<Func<TModel, bool>> filter)
    {
        return _mongoCollection.CountDocuments(filter);
    }

    /// <summary>
    /// Get List
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter)
    {
        return await GetListAsync(filter, CancellationToken.None);
    }

    /// <summary>
    /// Get List
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken)
    {
        return await (await _mongoCollection.FindAsync(filter, cancellationToken: cancellationToken)).ToListAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Get Paging List
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<TModel>> GetPagingListAsync(Expression<Func<TModel, bool>> filter, int page, int count, CancellationToken cancellationToken)
    {
        return await (await _mongoCollection.FindAsync(filter: filter, options: new FindOptions<TModel>
        {
            Skip = page * count,
            BatchSize = count
        }, cancellationToken)).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Get By ID
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<TModel> GetByIdAsync(Expression<Func<TModel, bool>> filter)
    {
        return await GetByIdAsync(filter, CancellationToken.None);
    }

    /// <summary>
    /// Get By ID
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<TModel> GetByIdAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken)
    {
        return await (await _mongoCollection.FindAsync(filter, cancellationToken: cancellationToken)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<TModel> CreateAsync(TModel model)
    {
        await CreateAsync(model, CancellationToken.None);
        return model;
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken)
    {
        await _mongoCollection.InsertOneAsync(model, cancellationToken: cancellationToken);
        return model;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(Expression<Func<TModel, bool>> filter, TModel model)
    {
        await UpdateAsync(filter, model, CancellationToken.None);
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(Expression<Func<TModel, bool>> filter, TModel model, CancellationToken cancellationToken)
    {
        await _mongoCollection.ReplaceOneAsync(filter, model, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(Expression<Func<TModel, bool>> filter)
    {
        await DeleteAsync(filter, CancellationToken.None);
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken)
    {
        await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
    }

    /// <summary>
    /// Exists
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TModel, bool>> filter)
    {
        return await ExistsAsync(filter, CancellationToken.None);
    }

    /// <summary>
    /// Exists
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken)
    {
        return await _mongoCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken) > 0;
    }

    /// <summary>
    /// Count
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<long> CountAsync(Expression<Func<TModel, bool>> filter)
    {
        return await CountAsync(filter, CancellationToken.None);
    }

    /// <summary>
    /// Count
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<long> CountAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken)
    {
        return await _mongoCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }
}
