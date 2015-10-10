using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;

using WebShopCase.API.Data;
using WebShopCase.API.Data.Infrastructure;
using WebShopCase.API.Data.Repositories;
using WebShopCase.API.Domain;
using WebShopCase.API.Service;
using WebShopCase.API.Core.Controllers;

namespace WebShopCase.API.Tests
{
    [TestFixture]
    public class TestController
    {
        #region Variables
            IProductService _productService;
            IProductRepository _productRepository;
            ICustomerRepository _customerRepository;

            IOrderService _orderService;
            IOrderRepository _orderRepository;
            ICustomerService _customerService;

            IUnitOfWork _unitOfWork;

            List<Product> _randomProducts;
            List<Order> _randomOrders;
            List<Customer> _randomCustomers;

            string _dummyAddress;
        #endregion

        #region Setup
            [SetUp]
            public void Setup()
            {
                _unitOfWork = new Mock<IUnitOfWork>().Object;
                
                _randomProducts = SetupProducts();
                _productRepository = SetupProductRepository();

                _randomOrders = NorthwindInitializer.GetOrders();
                _orderRepository = SetupOrderRepository();

                _randomCustomers = NorthwindInitializer.GetCustomers();
                _customerRepository = SetupCustomerRepository();

                _productService = new ProductService(_productRepository, _unitOfWork);
                _orderService = new OrderService(_orderRepository, _unitOfWork);
                _customerService = new CustomerService(_customerRepository, _unitOfWork);

                _dummyAddress = "http://test.com/";
            }

            public List<Product> SetupProducts()
            {
                int _counter = new int();
                List<Product> _prods = NorthwindInitializer.GetProducts();

                foreach (Product _product in _prods)
                    _product.ProductID = ++_counter;

                return _prods;
            }

            public IProductRepository SetupProductRepository()
            {
                // Init repository
                var repo = new Mock<IProductRepository>();

                // Setup mocking behavior
                repo.Setup(r => r.GetAll()).Returns(_randomProducts);

                /*
                repo.Setup(r => r.GetById(It.IsAny<int>()))
                    .Returns(new Func<int, Product>(
                        id => _randomProducts.Find(a => a.ProductID.Equals(id))));
                */

                return repo.Object;
            }

            public IOrderRepository SetupOrderRepository()
            {
                var repoMock = new Mock<IOrderRepository>();

                repoMock.Setup(r => r.Add(It.IsAny<Order>()))
               .Callback(new Action<Order>(newOrder =>
               {
                   dynamic maxOrderID = _randomOrders.Last().OrderID;
                   newOrder.OrderID = maxOrderID + 1;
                   _randomOrders.Add(newOrder);
               })
                );

                repoMock.Setup(r => r.GetAll()).
                    Returns(_randomOrders);

                repoMock.Setup(
                    moq => moq.GetMany(
                        It.IsAny<Expression<Func<Order, bool>>>()
                       , It.IsAny<Expression<Func<Order, object>>[]>()
                    ))
                    .Returns<Expression<Func<Order, bool>>, Expression<Func<Order, object>>[]>
                    (
                        (where, navProps) => _randomOrders.AsQueryable().IncludeMultiple(navProps).Where(where).ToList()
                    );

                return repoMock.Object;
            }

            public ICustomerRepository SetupCustomerRepository()
            {
                var repoMock = new Mock<ICustomerRepository>();

                repoMock.Setup(r => r.Add(It.IsAny<Customer>()))
                  .Callback(new Action<Customer>(newCustomer =>
                  {
                      dynamic maxCustomerID = _randomCustomers.Last().CustomerID;
                      newCustomer.CustomerID = maxCustomerID + 1;
                      _randomCustomers.Add(newCustomer);
                  })
                );

                repoMock.Setup(
                   moq => moq.GetSingle(
                       It.IsAny<Expression<Func<Customer, bool>>>()
                      , It.IsAny<Expression<Func<Customer, object>>[]>()
                   ))
                   .Returns<Expression<Func<Customer, bool>>, Expression<Func<Customer, object>>[]>
                   (
                       (where, navProps) => _randomCustomers.AsQueryable().IncludeMultiple(navProps).FirstOrDefault(where)
                   );

                return repoMock.Object;
            }
        #endregion

        #region Tests

            [Test]
            public void Controller_Should_Return_All_Products()
            {
                // Arrange
                var _articlesController = new ProductsController(_productService);

                // Act
                var result = _articlesController.GetAll();

                var dtos = from p in _randomProducts
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

                // Assert
                CollectionAssert.AreEqual(result, dtos);
            }

            [Test]
            public void Controller_Should_Post_NewOrder() 
            { 
                // Arrange
                var _newOrderDet = new OrderDetail()
                {
                    ProductID = 1,
                    Quantity = 2,
                    UnitPrice = (decimal)5.0,
                    Discount = (float)1
                };

                var _newOrdDetList = new List<OrderDetail>();

                _newOrdDetList.Add(_newOrderDet);

                var _newOrder = new Order()
                {
                    OrderDate = DateTime.Now,
                    RequiredDate = DateTime.Now,
                    ShipAddress = "ShipAddress1",
                    ShipCity = "ShipCity1",
                    ShipName = "ShipName1",
                    ShippedDate = DateTime.Now,
                    ShipPostalCode = "20100",
                    ShipCountry = "ShipCountry1",

                    ShipperID = 1,

                    CustomerID = 1,

                    OrderDetails = _newOrdDetList
                };

                // Act
                var _ordersController = new OrdersController(_orderService)
                {
                    Configuration = new HttpConfiguration(),
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(_dummyAddress + "/api/orders")
                    }
                };

                _ordersController.Configuration.MapHttpAttributeRoutes();
                _ordersController.Configuration.EnsureInitialized();
                _ordersController.RequestContext.RouteData =
                    new HttpRouteData(
                        new HttpRoute(),
                        new HttpRouteValueDictionary { { "_ordersController", "Orders" } });

                var result = _ordersController.PostOrder(_newOrder) as CreatedAtRouteNegotiatedContentResult<Order>;


                // Assert
                Assert.That(result.RouteName, Is.EqualTo("DefaultApi"));
                Assert.That(result.Content.OrderID, Is.EqualTo(result.RouteValues["id"]));
                Assert.That(result.Content.OrderID, Is.EqualTo(_randomOrders.Max(a => a.OrderID)));
            }

            //[Test]
            //public void Controller_Should_Get_Customer_ByPost()
            //{
            //    // Arrange
            //    string usr = "cus1@contoso.com";
            //    string pwd = "123";

            //    // Act
            //    var _customersController = new CustomersController(_customerService)
            //    {
            //        Configuration = new HttpConfiguration(),
            //        Request = new HttpRequestMessage
            //        {
            //            Method = HttpMethod.Post,
            //            RequestUri = new Uri(_dummyAddress + "/api/customers")
            //        }
            //    };

            //    var result = _customersController.GetCustomerByUserNamePassword(usr, pwd) as OkNegotiatedContentResult<Customer>;

            //    // Assert
            //    Assert.That(result.Content.CustomerID, Is.EqualTo(1));
            //}

            //[Test]
            //public void Controller_Should_Post_Customer()
            //{
            //    // Arrange
            //    string usr = "cus1@contoso.com";
            //    string pwd = "123";

            //    // Act
            //    var _customersController = new CustomersController(_customerService)
            //    {
            //        Configuration = new HttpConfiguration(),
            //        Request = new HttpRequestMessage
            //        {
            //            Method = HttpMethod.Post,
            //            RequestUri = new Uri(_dummyAddress + "/api/customers")
            //        }
            //    };

            //    var result = _customersController.PostCustomer(new Customer() { UserName = usr, Password = pwd }) as CreatedAtRouteNegotiatedContentResult<Customer>;

            //    // Assert
            //    Assert.That(result.RouteName, Is.EqualTo("DefaultApi"));
            //    Assert.That(result.Content.CustomerID, Is.EqualTo(result.RouteValues["id"]));
            //    Assert.That(result.Content.CustomerID, Is.EqualTo(_randomCustomers.Max(c => c.CustomerID)));
   
            //}
                

        #endregion

        /*
        [TestMethod]
        public void GetReturnsProduct()
        {
            // Arrange
            var controller = new ProductController(;
            
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(10);

            // Assert
            Product product;
            Assert.IsTrue(response.TryGetContentValue<Product>(out product));
            Assert.AreEqual(10, product.Id);
        }
         */
    }
}
