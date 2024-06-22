using CodeNet.EntityFramework.Repositories;

namespace StokTakip.Customer.Abstraction.Repository;

public interface ICustomerRepository : ITracingRepository<Model.Customer>
{
}
