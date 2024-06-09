using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System.Linq.Expressions;

namespace NetCore.MongoDB;

/// <summary>
/// MongoDB Repository
/// </summary>
/// <typeparam name="TModel"></typeparam>
public class BaseMongoRepository<TModel> : IMongoDBRepository<TModel>, IMongoDBAsyncRepository<TModel> where TModel : class, new()
{
    private readonly IMongoCollection<TModel> _mongoCollection;
    private readonly MongoDBSettings _settings;

    /// <summary>
    /// MongoDB Repository
    /// </summary>
    /// <param name="options"></param>
    public BaseMongoRepository(IOptions<MongoDBSettings> options)
    {
        _settings = options.Value;
        var client = new MongoClient(_settings.ConnectionString);
        var databaseName = string.IsNullOrEmpty(_settings.DatabaseName) ? MongoUrl.Create(_settings.ConnectionString).DatabaseName : _settings.DatabaseName;
        var database = client.GetDatabase(databaseName);
        _mongoCollection = database.GetCollection<TModel>(_settings.CollectionName);
    }

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
        return (await _mongoCollection.FindAsync(filter)).ToList();
    }

    /// <summary>
    /// Get By ID
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<TModel> GetByIdAsync(Expression<Func<TModel, bool>> filter)
    {
        return (await _mongoCollection.FindAsync(filter)).FirstOrDefault();
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<TModel> CreateAsync(TModel model)
    {
        await _mongoCollection.InsertOneAsync(model);
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
        await _mongoCollection.ReplaceOneAsync(filter, model);
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(Expression<Func<TModel, bool>> filter)
    {
        await _mongoCollection.DeleteOneAsync(filter);
    }

    /// <summary>
    /// Exists
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TModel, bool>> filter)
    {
        return (await _mongoCollection.CountDocumentsAsync(filter)) > 0;
    }

    /// <summary>
    /// Count
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<long> CountAsync(Expression<Func<TModel, bool>> filter)
    {
        return (await _mongoCollection.CountDocumentsAsync(filter));
    }
}
