using Net5Api.Abstraction;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Collections.Generic;
using System.Linq;

namespace StokTakip.Service
{
    public class CustomerService : BaseService, ICustomerService
    {
        ICustomerRepository customerRepository;
        ILogRepository logRepository;
        IQService qService;

        public CustomerService(ICustomerRepository customerRepository, ILogRepository logRepository, IQService qService)
        {
            this.customerRepository = customerRepository;
            this.qService = qService;
            this.logRepository = logRepository;
        }

        public CustomerViewModel CreateCustomer(CustomerViewModel customer)
        {
            customerRepository.SetUser(GetUser());
            var result = customerRepository.Add(new EntityFramework.Models.Customer
            {
                Code = customer.Code,
                Description = customer.Description,
                Name = customer.Name,
                No = customer.No,
                IsActive = true,
                IsDeleted = false
            });
            return new CustomerViewModel
            {
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No
            };
        }

        public CustomerViewModel DeleteCustomer(int customerId)
        {
            customerRepository.SetUser(GetUser());
            var result = customerRepository.Get(customerId);
            result.IsDeleted = true;
            customerRepository.Update(result);
            return new CustomerViewModel
            {
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No
            };
        }

        public CustomerViewModel GetCustomer(int customerId)
        {
            customerRepository.SetUser(GetUser());
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
            qService.Post("GetCustomer", value);
            return value;
        }

        public List<CustomerViewModel> GetCustomers()
        {
            customerRepository.SetUser(GetUser());
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
            qService.Post("GetCustomers", result);
            return result;
        }

        public CustomerViewModel UpdateCustomer(CustomerViewModel customer)
        {
            customerRepository.SetUser(GetUser());
            var result = customerRepository.Get(customer.Id);
            result.Code = customer.Code;
            result.Description = customer.Description;
            result.Name = customer.Name;
            result.No = customer.No;
            customerRepository.Update(result);
            return new CustomerViewModel
            {
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                No = result.No
            };
        }
    }
}
