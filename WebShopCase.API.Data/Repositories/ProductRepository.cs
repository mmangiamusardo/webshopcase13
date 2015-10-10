using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCase.API.Domain;
using WebShopCase.API.Data.Infrastructure;

namespace WebShopCase.API.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbfactory) : base(dbfactory) { }
    }

    public interface IProductRepository : IRepository<Product>{}
}
