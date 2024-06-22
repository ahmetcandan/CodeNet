using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CodeNet.MongoDB.Settings;
using CodeNet.MongoDB.Models;

namespace CodeNet.MongoDB;

public class MongoDBContext
{
    protected readonly IMongoDatabase _database;

    public MongoDBContext(IOptions<MongoDBSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var databaseName = string.IsNullOrEmpty(options.Value.DatabaseName) ? MongoUrl.Create(options.Value.ConnectionString).DatabaseName : options.Value.DatabaseName;
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<TModel> Set<TModel>() where TModel : class, IBaseMongoDBModel
    {
        string collectionName = (typeof(TModel).GetCustomAttributes(typeof(CollectionNameAttribute), true).FirstOrDefault() is not CollectionNameAttribute collectionAttribute)
                ? typeof(TModel).Name
                : collectionAttribute.Name;
        return _database.GetCollection<TModel>(collectionName);
    }
}
