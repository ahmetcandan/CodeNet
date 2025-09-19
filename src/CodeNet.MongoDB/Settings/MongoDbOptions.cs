using CodeNet.MongoDB.Repositories;

namespace CodeNet.MongoDB.Settings;

public class MongoDbOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}

public class MongoDbOptions<TDbContext> : MongoDbOptions
    where TDbContext : MongoDBContext
{
}
