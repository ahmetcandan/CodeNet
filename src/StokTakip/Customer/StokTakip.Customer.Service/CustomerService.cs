using NetCore.Abstraction;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;
using System.Reflection;

namespace StokTakip.Customer.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAppLogger _appLogger;

        public CustomerService(ICustomerRepository customerRepository, IAppLogger appLogger)
        {
            _customerRepository = customerRepository;
            _appLogger = appLogger;
        }

        public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.AddAsync(new Model.Customer
            {
                Code = request.Code,
                Description = request.Description,
                Name = request.Name,
                No = request.No,
                IsActive = true,
                IsDeleted = false
            }, cancellationToken);
            await _customerRepository.SaveChangesAsync(cancellationToken);
            var response = new CustomerResponse
            {
                Id = result.Id,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No
            };
            return response;
        }

        public async Task<CustomerResponse> DeleteCustomer(int customerId, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.GetAsync([customerId], cancellationToken);
            result.IsDeleted = true;
            _customerRepository.Update(result);
            await _customerRepository.SaveChangesAsync(cancellationToken);
            return new CustomerResponse
            {
                Id = result.Id,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No
            };
        }

        public async Task<CustomerResponse> GetCustomer(int customerId, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.GetAsync([customerId], cancellationToken);
            if (result == null)
                return null;

            _appLogger.TraceLog(result, MethodBase.GetCurrentMethod());
            var value = new CustomerResponse
            {
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No,
                Id = result.Id
            };
            return value;
        }

        public async Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.GetAsync([request.Id], cancellationToken);
            result.Code = request.Code;
            result.Description = request.Description;
            result.Name = request.Name;
            result.No = request.No;
            _customerRepository.Update(result);
            await _customerRepository.SaveChangesAsync(cancellationToken);
            return new CustomerResponse
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
