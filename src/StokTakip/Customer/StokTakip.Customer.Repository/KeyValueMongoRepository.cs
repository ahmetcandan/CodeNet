using CodeNet.MongoDB;
using CodeNet.MongoDB.Repositories;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Repository;

public class KeyValueMongoRepository(MongoDBContext dbContext) : BaseMongoRepository<MongoModel>(dbContext), IKeyValueRepository
{
}
