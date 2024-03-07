using NetCore.Abstraction;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;
using StokTakip.Customer.Service.Mapper;

namespace StokTakip.Customer.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAutoMapperConfiguration _mapper;

        public CustomerService(ICustomerRepository customerRepository, IAutoMapperConfiguration mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            var model = _mapper.MapObject<CreateCustomerRequest, Model.Customer>(request);
            var result = await _customerRepository.AddRangeAsync([model], cancellationToken);
            await _customerRepository.SaveChangesAsync(cancellationToken);
            return _mapper.MapObject<Model.Customer, CustomerResponse>(result.FirstOrDefault());
        }

        public async Task<CustomerResponse> DeleteCustomer(int customerId, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.GetAsync([customerId], cancellationToken);
            _customerRepository.Remove(result);
            await _customerRepository.SaveChangesAsync(cancellationToken);
            return _mapper.MapObject<Model.Customer, CustomerResponse>(result);
        }

        public async Task<CustomerResponse?> GetCustomer(int customerId, CancellationToken cancellationToken)
        {
            var result = await _customerRepository.Find(c => c.Id == customerId,  cancellationToken);
            if (result?.Any() == false)
                return null;

            return _mapper.MapObject<Model.Customer, CustomerResponse>(result.First());
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
            return _mapper.MapObject<Model.Customer, CustomerResponse>(result);
        }
    }
}
