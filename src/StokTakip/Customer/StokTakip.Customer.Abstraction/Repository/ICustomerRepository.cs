using NetCore.Abstraction;

namespace StokTakip.Customer.Abstraction.Repository;

public interface ICustomerRepository : ITracingRepository<Model.Customer>
{
}
