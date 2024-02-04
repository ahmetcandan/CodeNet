using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Customer.Api.Controllers
{
    [Authorize(Roles = "customer")]
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService ?? throw new System.ArgumentNullException(nameof(customerService));
        }

        [HttpGet("{customerId}")]
        public async Task<CustomerViewModel> Get(int customerId, CancellationToken cancellationToken)
        {
            return await _customerService.GetCustomer(customerId, cancellationToken);
        }

        [HttpPost]
        public async Task<CustomerViewModel> Post(CustomerViewModel customer, CancellationToken cancellationToken)
        {
            return await _customerService.CreateCustomer(customer, cancellationToken);
        }

        [HttpPut]
        public async Task<CustomerViewModel> Put(CustomerViewModel customer, CancellationToken cancellationToken)
        {
            return await _customerService.UpdateCustomer(customer, cancellationToken);
        }

        [HttpDelete]
        public async Task<CustomerViewModel> Delete(int customerId, CancellationToken cancellationToken)
        {
            return await _customerService.DeleteCustomer(customerId, cancellationToken);
        }
    }
}
