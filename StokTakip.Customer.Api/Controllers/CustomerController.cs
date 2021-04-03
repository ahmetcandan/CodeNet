using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StokTakip.Abstraction;
using StokTakip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace StokTakip.Customer.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        ICustomerService CustomerService;

        public CustomerController(ICustomerService customerService)
        {
            CustomerService = customerService;
        }

        [HttpGet]
        public List<CustomerViewModel> GetAll()
        {
            CustomerService.SetUser(User);
            return CustomerService.GetCustomers();
        }

        [HttpGet("{customerId}")]
        public CustomerViewModel Get(int customerId)
        {
            CustomerService.SetUser(User);
            return CustomerService.GetCustomer(customerId);
        }

        [HttpPost]
        public CustomerViewModel Post(CustomerViewModel customer)
        {
            CustomerService.SetUser(User);
            return CustomerService.CreateCustomer(customer);
        }

        [HttpPut]
        public CustomerViewModel Put(CustomerViewModel customer)
        {
            CustomerService.SetUser(User);
            return CustomerService.UpdateCustomer(customer);
        }

        [HttpDelete]
        public CustomerViewModel Delete(int customerId)
        {
            CustomerService.SetUser(User);
            return CustomerService.DeleteCustomer(customerId);
        }
    }
}
