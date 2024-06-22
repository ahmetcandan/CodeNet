using CodeNet.MongoDB.Repositories;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Abstraction.Repository;

public interface IKeyValueRepository : IMongoDBRepository<MongoModel>
{
}
