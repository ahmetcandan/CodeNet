using NetCore.Abstraction;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Abstraction.Repository;

public interface IKeyValueRepository : IMongoDBRepository<KeyValueModel>
{
}
