using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StokTakip.Abstraction;
using StokTakip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        public List<CustomerViewModel> GetAll()
        {
            return customerService.GetCustomers();
        }

        //[HttpGet]
        //public CustomerViewModel Get(int customerId)
        //{
        //    return customerService.GetCustomer(customerId);
        //}

        [HttpPost]
        public CustomerViewModel Post(CustomerViewModel customer)
        {
            return customerService.CreateCustomer(customer);
        }

        [HttpPut]
        public CustomerViewModel Put(CustomerViewModel customer)
        {
            return customerService.UpdateCustomer(customer);
        }

        [HttpDelete]
        public CustomerViewModel Delete(int customerId)
        {
            return customerService.DeleteCustomer(customerId);
        }
    }
}
