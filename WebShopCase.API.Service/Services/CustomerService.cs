using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCase.API.Data.Infrastructure;
using WebShopCase.API.Data.Repositories;
using WebShopCase.API.Domain;

namespace WebShopCase.API.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;

        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            this.customerRepository = customerRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICustomerService Members

            public Customer GetCustomerById(int id)
            {
                return customerRepository.GetSingle(c => c.CustomerID == id);
            }

            public CustomerDTO GetCustomerByUserName(string userName)
            {
                var cus = customerRepository.GetSingle(c => c.AppUser.UserName == userName, c => c.AppUser);
                return new CustomerDTO()
                {
                    CustomerID = cus.CustomerID,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    Address = cus.Address,
                    PostalCode = cus.PostalCode,
                    City = cus.City,
                    Country = cus.Country,
                    UserName = cus.AppUser.UserName,
                    AppUserID = cus.AppUser.Id
                };
            }

            public void CreateCustomer(Customer customer)
            {
                customerRepository.Add(customer);
                unitOfWork.Commit();
            }

            public void UpdateCustomer(Customer customer) 
            {
                customerRepository.Update(customer);
            }    

            public void SaveCustomer()
            {
                unitOfWork.Commit();
            }

        #endregion
        
    }

    public interface ICustomerService 
    {
        Customer GetCustomerById(int id);
        CustomerDTO GetCustomerByUserName(string userName);
        void CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void SaveCustomer();
    }
}
