using MongoDB.Driver;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System.Linq.Expressions;

namespace NetCore.MongoDB;

public class BaseMongoRepository<TModel> : INoSqlRepository<TModel>, INoSqlAsyncRepository<TModel> where TModel : INoSqlModel, new()
{
    private readonly IMongoCollection<TModel> _mongoCollection;

    public BaseMongoRepository(string mongoDBConnectionString, string dbName, string collectionName)
    {
        var client = new MongoClient(mongoDBConnectionString);
        var database = client.GetDatabase(dbName);
        _mongoCollection = database.GetCollection<TModel>(collectionName);
    }

    public virtual List<TModel> GetList()
    {
        return _mongoCollection.Find(c => true).ToList();
    }

    public virtual List<TModel> GetList(Expression<Func<TModel, bool>> filter)
    {
        return _mongoCollection.Find(filter).ToList();
    }

    public virtual TModel GetById(string id)
    {
        return _mongoCollection.Find(m => m.Id == id).FirstOrDefault();
    }

    public virtual TModel Create(TModel model)
    {
        _mongoCollection.InsertOne(model);
        return model;
    }

    public virtual void Update(string id, TModel model)
    {
        _mongoCollection.ReplaceOne(m => m.Id == id, model);
    }

    public virtual void Delete(TModel model)
    {
        _mongoCollection.DeleteOne(m => m.Id == model.Id);
    }

    public virtual void Delete(string id)
    {
        _mongoCollection.DeleteOne(m => m.Id == id);
    }

    public virtual bool ContainsId(string id)
    {
        return _mongoCollection.Count(c => c.Id == id) > 0;
    }

    public virtual async Task<List<TModel>> GetListAsync()
    {
        return (await _mongoCollection.FindAsync(c => true)).ToList();
    }

    public virtual async Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter)
    {
        return (await _mongoCollection.FindAsync(filter)).ToList();
    }

    public virtual async Task<TModel> GetByIdAsync(string id)
    {
        return (await _mongoCollection.FindAsync(m => m.Id == id)).FirstOrDefault();
    }

    public virtual async Task<TModel> CreateAsync(TModel model)
    {
        await _mongoCollection.InsertOneAsync(model);
        return model;
    }

    public virtual async Task UpdateAsync(string id, TModel model)
    {
        await _mongoCollection.ReplaceOneAsync(m => m.Id == id, model);
    }

    public virtual async Task DeleteAsync(TModel model)
    {
        await _mongoCollection.DeleteOneAsync(m => m.Id == model.Id);
    }

    public virtual async Task DeleteAsync(string id)
    {
        await _mongoCollection.DeleteOneAsync(m => m.Id == id);
    }

    public virtual async Task<bool> ContainsIdAsync(string id)
    {
        return (await _mongoCollection.CountAsync(c => c.Id == id)) > 0;
    }
}
