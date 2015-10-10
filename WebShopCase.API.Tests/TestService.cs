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

namespace WebShopCase.API.Tests
{
    [TestFixture]
    public class TestService
    {
        #region Variables
            IProductRepository _productRepository;
            IOrderRepository _orderRepository;
            ICustomerRepository _customerRepository;

            IUnitOfWork _unitOfWork;

            List<Product> _randomProducts;
            List<Order> _randomOrders;
            List<Customer> _randomCustomers;

        #endregion

        #region Setup
            [SetUp]
            public void Setup()
            {
                _randomOrders = NorthwindInitializer.GetOrders();
                _orderRepository = SetupOrderRepository();

                _randomProducts = SetupProducts();
                _productRepository = SetupProductRepository();

                _randomCustomers = NorthwindInitializer.GetCustomers();
                _customerRepository = SetupCustomerRepository();

                _unitOfWork = new Mock<IUnitOfWork>().Object;
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
                var repoMock = new Mock<IProductRepository>();

                // Setup mocking behavior
                repoMock.Setup(r => r.GetAll()).Returns(_randomProducts);
                
                repoMock.Setup(
                    moq => moq.GetSingle(
                        It.IsAny<Expression<Func<Product, bool>>>()
                       ,It.IsAny<Expression<Func<Product, object>>[]>()
                    ))
                    .Returns<Expression<Func<Product, bool>>, Expression<Func<Product, object>>[]>
                    (
                        (where, navProps) => _randomProducts.AsQueryable().IncludeMultiple(navProps).FirstOrDefault(where)
                    );
                
                return repoMock.Object;
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
                       ,It.IsAny<Expression<Func<Order, object>>[]>()
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
            public void Service_Should_Return_All_Products()
            {
                // Arrange
                var productService = new ProductService(_productRepository, _unitOfWork);

                // Act
                var result = productService.GetAllProducts();

                // Assert
                //Assert.That(prods, Is.EqualTo(_randomProducts));
                for (int i = 0; i < _randomProducts.Count; i++)
                {
                    Assert.AreEqual(result.ToList()[i].ProductID, _randomProducts[i].ProductID);
                }
            }

            [Test]
            public void Service_Should_Return_ProductId_2() 
            {
                // Arrange
                var productService = new ProductService(_productRepository, _unitOfWork);

                // Act
                var result = productService.GetProductById(2);

                // Assert
                Assert.That(result.ProductID, 
                    Is.EqualTo(_randomProducts.Find(p => p.ProductName == "prod2").ProductID));
               }

            [Test]
            public void Service_Should_Return_Product_prod16()
            {
                // Arrange
                var productService = new ProductService(_productRepository, _unitOfWork);

                // Act
                var result = productService.GetProductByName("prod16");

                // Assert
                Assert.That(result.ProductID, Is.EqualTo(_randomProducts.Find(p => p.ProductID == 16).ProductID));
            }

            [Test]
            public void Service_Should_Return_Paged_Products_page3_7products()
            {
                // Arrange
                var productService = new ProductService(_productRepository, _unitOfWork);
                int pageIndex = 1;
                int pageSize = 7;

                //Act
                var allProds = productService.GetAllProducts().ToList();
                var pagedProds = productService.GetPagedProducts(pageIndex, pageSize);
                
                // Assert
                Assert.AreEqual(7, pagedProds.Count);
                
                for (int i = 0; i < pagedProds.Count; i++)
                {
                    Assert.AreEqual(allProds[pageIndex * pageSize + i].ProductID, pagedProds.Items.ToList()[i].ProductID);
                }
            
            }

            [Test]
            public void Service_Should_Add_NewOrder()
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

                int _maxOrderIDBeforeAdd = _randomOrders.Max(o => o.OrderID);

                var _orderService = new OrderService(_orderRepository, _unitOfWork);

                // Act
                _orderService.CreateOrder(_newOrder);

                // Assert
                Assert.That(_newOrder, Is.EqualTo(_randomOrders.Last()));
                Assert.That(_maxOrderIDBeforeAdd + 1, Is.EqualTo(_randomOrders.Last().OrderID));
            }

            //[Test]
            //public void Service_Should_Get_Orders_By_Customer_1() 
            //{ 
            //    // Arrange
            //    var _orderService = new OrderService(_orderRepository, _unitOfWork);

            //    // Act
            //    var _ordersByCustomer = _orderService.GetOrdersByCustomer(1);

            //    string login = "cus1@contoso.com";

            //    // Assert
            //    Assert.That(login, Is.EqualTo(_ordersByCustomer.FirstOrDefault().Customer.Username));
            //}

            //[Test]
            //public void Service_Should_Get_Customer_ByUserNamePassword_CustomerID_1() 
            //{
            //    // Arrange
            //    var _customerService = new CustomerService(_customerRepository, _unitOfWork);
         
            //    // Act
            //    var _customer = _customerService.GetCustomerByUserNamePassword("cus1@contoso.com", "123");

            //    int customerID = 1;

            //    // Assert
            //    Assert.That(customerID, Is.EqualTo(_customer.CustomerID));
            //}

            //[Test]
            //public void Service_Should_Get_Nulls_Customer_ByUserNamePassword()
            //{
            //    // Arrange
            //    var _customerService = new CustomerService(_customerRepository, _unitOfWork);

            //    // Act
            //    var _customer = _customerService.GetCustomerByUserNamePassword("notexists@contoso.com", "123");

            //    // Assert
            //    Assert.IsNull(_customer);
            //}
        
        #endregion
    }
}