using CodeNet.MongoDB.Attributes;
using CodeNet.MongoDB.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CodeNet.MongoDB.Repositories;

public class MongoDBContext
{
    protected readonly IMongoDatabase _database;

    public MongoDBContext(IOptions<MongoDbOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var databaseName = string.IsNullOrEmpty(options.Value.DatabaseName) ? MongoUrl.Create(options.Value.ConnectionString).DatabaseName : options.Value.DatabaseName;
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<TModel> Set<TModel>() where TModel : class
    {
        string collectionName = typeof(TModel).GetCustomAttributes(typeof(CollectionNameAttribute), true).FirstOrDefault() is not CollectionNameAttribute collectionAttribute
                ? typeof(TModel).Name
                : collectionAttribute.Name;
        return _database.GetCollection<TModel>(collectionName);
    }

    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _database.RunCommandAsync((Command<BsonDocument>)"{ ping: 1 }", cancellationToken: cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}