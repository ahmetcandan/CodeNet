using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NetCore.Abstraction.Model;

namespace NetCore.Abstraction;

public class MongoDBContext
{
    protected readonly IMongoDatabase _database;

    public MongoDBContext(IOptions<MongoDBSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var databaseName = string.IsNullOrEmpty(options.Value.DatabaseName) ? MongoUrl.Create(options.Value.ConnectionString).DatabaseName : options.Value.DatabaseName;
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<TModel> Set<TModel>(string collectionName) where TModel : class, IBaseMongoDBModel
    {
        return _database.GetCollection<TModel>(collectionName);
    }
}
