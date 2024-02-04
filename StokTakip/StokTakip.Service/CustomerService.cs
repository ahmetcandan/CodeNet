using NetCore.Abstraction;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerViewModel> CreateCustomer(CustomerViewModel customer, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.AddAsync(new EntityFramework.Models.Customer
            {
                Code = customer.Code,
                Description = customer.Description,
                Name = customer.Name,
                No = customer.No,
                IsActive = true,
                IsDeleted = false
            }, cancellationToken);
            await _customerRepository.SaveChangesAsync(cancellationToken);
            var response = new CustomerViewModel
            {
                Id = result.Id,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No
            };
            return response;
        }

        public async Task<CustomerViewModel> DeleteCustomer(int customerId, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.GetAsync(customerId, cancellationToken);
            result.IsDeleted = true;
            _customerRepository.Update(result);
            await _customerRepository.SaveChangesAsync(cancellationToken);
            return new CustomerViewModel
            {
                Id = result.Id,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No
            };
        }

        public async Task<CustomerViewModel> GetCustomer(int customerId, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.GetAsync(customerId, cancellationToken);
            if (result == null)
                return null;
            var value = new CustomerViewModel
            {
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No,
                Id = result.Id
            };
            return value;
        }

        public async Task<CustomerViewModel> UpdateCustomer(CustomerViewModel customer, CancellationToken cancellationToken)
        {
            var result = _customerRepository.Get(customer.Id);
            result.Code = customer.Code;
            result.Description = customer.Description;
            result.Name = customer.Name;
            result.No = customer.No;
            _customerRepository.Update(result);
            await _customerRepository.SaveChangesAsync(cancellationToken);
            return new CustomerViewModel
            {
                Id = result.Id,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No
            };
        }
    }
}
