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
    public interface IProductService
    {
        IList<ProductDTO> GetAllProducts();
        PagedCollection<ProductDTO> GetPagedProducts(int? page, int? pageSize);
        ProductDTO GetProductById(int id);
        ProductDTO GetProductByName(string name);
    }
    
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this.productRepository = productRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IProductService Members

            public IList<ProductDTO> GetAllProducts()
            {
                var dtos = from p in productRepository.GetAll()
                           select new ProductDTO()
                           {
                               ProductID = p.ProductID
                               ,
                               ProductName = p.ProductName
                               ,
                               UnitPrice = p.UnitPrice
                               ,
                               ProductPct = p.Picture
                           };

                return dtos.ToList();

            }

            public PagedCollection<ProductDTO> GetPagedProducts(int? page, int? pageSize) 
            {
                var currPage = page.GetValueOrDefault(0);
                var currPageSize = pageSize.HasValue && pageSize.Value > 0 ? pageSize.Value : 10;

                var paged = productRepository.GetAll(p => p.Category).Skip(currPage * currPageSize)
                                    .Take(currPageSize)
                                    .ToArray();

                var totalCount = productRepository.GetAll().Count();

                return new PagedCollection<ProductDTO>()
                {
                    Page = currPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currPageSize),
                    Items = from p in paged
                            select new ProductDTO()
                            {
                                ProductID = p.ProductID
                                ,
                                ProductName = p.ProductName
                                ,
                                UnitPrice = p.UnitPrice
                                ,
                                ProductPct = p.Picture
                                ,
                                VAT = p.Category.VAT
                            }
                };
            }

            public ProductDTO GetProductById(int id)
            {
                var prod = productRepository.GetSingle(p => p.ProductID == id, p => p.Category);
                
                return new ProductDTO()
                {
                    ProductID = prod.ProductID
                    ,
                    ProductName = prod.ProductName
                    ,
                    QuantityPerUnit = prod.QuantityPerUnit
                    ,
                    UnitPrice = prod.UnitPrice
                    ,
                    ProductPct = prod.Picture
                    ,
                    CategoryName = prod.Category.CategoryName
                    ,
                    CategoryPct = prod.Category.Picture.ToBase64()
                    ,
                    UnitsInStock = prod.UnitsInStock
                    ,
                    UnitsOnOrder = prod.UnitsOnOrder
                    ,
                    VAT = prod.Category.VAT
                };
            }

            public ProductDTO GetProductByName(string name)
            {
                var prod = productRepository.GetSingle(p => p.ProductName == name, p => p.Category);

                return new ProductDTO()
                {
                    ProductID = prod.ProductID
                    ,
                    ProductName = prod.ProductName
                    ,
                    QuantityPerUnit = prod.QuantityPerUnit
                    ,
                    UnitPrice = prod.UnitPrice
                    ,
                    ProductPct = prod.Picture
                    ,
                    CategoryName = prod.Category.CategoryName
                    ,
                    CategoryPct = prod.Category.Picture.ToBase64()
                    ,
                    UnitsInStock = prod.UnitsInStock
                    ,
                    UnitsOnOrder = prod.UnitsOnOrder
                    ,
                    VAT = prod.Category.VAT
                };
            }

        #endregion    

    }
}
