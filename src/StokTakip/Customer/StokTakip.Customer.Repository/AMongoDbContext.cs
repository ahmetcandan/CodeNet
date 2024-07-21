using CodeNet.MongoDB;
using CodeNet.MongoDB.Settings;
using Microsoft.Extensions.Options;

namespace StokTakip.Customer.Repository;

public class AMongoDbContext(IOptions<MongoDbOptions<AMongoDbContext>> options) : MongoDBContext(options)
{
}

public class BMongoDbContext(IOptions<MongoDbOptions<BMongoDbContext>> options) : MongoDBContext(options)
{
}
