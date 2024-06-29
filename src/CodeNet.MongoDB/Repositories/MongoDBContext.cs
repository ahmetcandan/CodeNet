using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CodeNet.MongoDB.Settings;
using CodeNet.MongoDB.Models;
using MongoDB.Bson;

namespace CodeNet.MongoDB;

public class MongoDBContext
{
    protected readonly IMongoDatabase _database;
    private readonly MongoDBSettings _settings;

    public MongoDBContext(IOptions<MongoDBSettings> options)
    {
        _settings = options.Value;
        var client = new MongoClient(_settings.ConnectionString);
        var databaseName = string.IsNullOrEmpty(_settings.DatabaseName) ? MongoUrl.Create(_settings.ConnectionString).DatabaseName : _settings.DatabaseName;
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<TModel> Set<TModel>() where TModel : class, IBaseMongoDBModel
    {
        string collectionName = (typeof(TModel).GetCustomAttributes(typeof(CollectionNameAttribute), true).FirstOrDefault() is not CollectionNameAttribute collectionAttribute)
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
