using Microsoft.EntityFrameworkCore;
using NetCore.Abstraction;
using StokTakip.Abstraction;
using StokTakip.Model;
using StokTakip.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace StokTakip.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogRepository logRepository;
        private readonly IQService qService;

        public CustomerService(DbContext dbContext, ILogRepository logRepository, IQService qService)
        {
            customerRepository = new CustomerRepository(dbContext);
            this.qService = qService;
            this.logRepository = logRepository;
        }

        public override void SetUser(IPrincipal user)
        {
            customerRepository.SetUser(user);
            base.SetUser(user);
        }

        public CustomerViewModel CreateCustomer(CustomerViewModel customer)
        {
            var result = customerRepository.Add(new EntityFramework.Models.Customer
            {
                Code = customer.Code,
                Description = customer.Description,
                Name = customer.Name,
                No = customer.No,
                IsActive = true,
                IsDeleted = false
            });
            customerRepository.SaveChanges();
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

        public CustomerViewModel DeleteCustomer(int customerId)
        {
            var result = customerRepository.Get(customerId);
            result.IsDeleted = true;
            customerRepository.Update(result);
            customerRepository.SaveChanges();
            return new CustomerViewModel
            {
                Id = result.Id,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No
            };
        }

        public CustomerViewModel GetCustomer(int customerId)
        {
            var result = customerRepository.Get(customerId);
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

        public List<CustomerViewModel> GetCustomers()
        {
            var result = (
                    from c in customerRepository.GetAll()
                    select new CustomerViewModel
                    {
                        Code = c.Code,
                        Description = c.Description,
                        Id = c.Id,
                        Name = c.Name,
                        No = c.No
                    }
                ).ToList();
            return result;
        }

        public CustomerViewModel UpdateCustomer(CustomerViewModel customer)
        {
            var result = customerRepository.Get(customer.Id);
            result.Code = customer.Code;
            result.Description = customer.Description;
            result.Name = customer.Name;
            result.No = customer.No;
            customerRepository.Update(result);
            customerRepository.SaveChanges();
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
