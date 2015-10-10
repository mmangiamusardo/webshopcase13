using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

using WebShopCase.API.Domain;
using WebShopCase.API.Service;

namespace WebShopCase.API.Core.Controllers
{
    public class ProductsController : ApiController
    {
        private IProductService _productService;

        public ProductsController(IProductService productService) 
        {
            _productService = productService;
        }
        
        public IEnumerable<ProductDTO> GetAll()
        {
            return _productService.GetAllProducts();
        }

        public IHttpActionResult GetById(int id)
        {
            ProductDTO prod = _productService.GetProductById(id);
            if (prod == null)
            {
                return NotFound();
            }

            return Ok(prod);
        }
       
        public PagedCollection<ProductDTO> GetPaged(int? pageIndex, int? pageSize)
        {
            return _productService.GetPagedProducts(pageIndex, pageSize);
        }
    }
}
