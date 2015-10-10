using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCase.API.Domain;
using WebShopCase.API.Data.Infrastructure;

namespace WebShopCase.API.Data.Repositories
{
    public class OrderRepository :  RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbfactory) 
            : base(dbfactory) { }

    }

    public interface IOrderRepository : IRepository<Order>{}
}
