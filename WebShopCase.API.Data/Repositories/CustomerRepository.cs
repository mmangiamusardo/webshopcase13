using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCase.API.Domain;
using WebShopCase.API.Data.Infrastructure;

namespace WebShopCase.API.Data.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDbFactory dbfactory) : base(dbfactory) { }
    }

    public interface ICustomerRepository : IRepository<Customer>{}
}
