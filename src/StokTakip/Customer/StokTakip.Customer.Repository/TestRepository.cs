using CodeNet.MongoDB;
using CodeNet.MongoDB.Repositories;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Repository;

public class TestRepository(MongoDBContext dbContext) : BaseMongoRepository<KeyValueModel>(dbContext)
{
    //...
}
