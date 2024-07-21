using CodeNet.MongoDB;
using CodeNet.MongoDB.Repositories;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Repository;

public class KeyValueMongoRepository(MongoDBContext dbContext) : BaseMongoRepository<MongoModel>(dbContext), IKeyValueRepository
{
}

public class AKeyValueMongoRepository(AMongoDbContext dbContext) : BaseMongoRepository<MongoModel>(dbContext), IAKeyValueRepository
{
}

public class BKeyValueMongoRepository(BMongoDbContext dbContext) : BaseMongoRepository<MongoModel>(dbContext), IBKeyValueRepository
{
}