using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using System.Configuration;

using WebShopCase.API.Data;
using WebShopCase.API.Data.Infrastructure;
using WebShopCase.API.Data.Repositories;
using WebShopCase.API.Domain;
using WebShopCase.API.Service;
using WebShopCase.API.Core.Controllers;

namespace WebShopCase.API.Tests
{
    [TestFixture]
    public class TestRoute
    {
        #region Variables
         HttpConfiguration _config;
         string _dummyAddress;
        #endregion

        #region Setup

            [SetUp]
            public void Setup()
            {
                _dummyAddress = "http://test.com/";

                _config = new HttpConfiguration();

                _config.Routes.MapHttpRoute(
                    name: "DefaultWebAPI",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional });

                 _config.Routes.MapHttpRoute(
                   name: "PagedItemsApi",
                   routeTemplate: "api/{controller}/page/{pageIndex}/{pageSize}",
                   defaults: new { action = "GetPaged" });
            }

        #endregion

        #region Tests
            [Test]
            public void Route_Should_Controller_GetProductById_IsInvoked()
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Get, _dummyAddress + "api/products/16");

                // Act
                var _actionSelector = new ControllerActionSelector(_config, request);

                // Assert
                Assert.That(typeof(ProductsController), Is.EqualTo(_actionSelector.GetControllerType()));
                Assert.That(GetMethodName((ProductsController c) => 
                    c.GetById(16)),
                    Is.EqualTo(_actionSelector.GetActionName()));
            }

            [Test]
            public void Route_Should_Controller_GetPagedProducts_IsInvoked()
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Get, _dummyAddress + "api/products/page/1/7");

                // Act
                var _actionSelector = new ControllerActionSelector(_config, request);

                // Assert
                Assert.That(typeof(ProductsController), Is.EqualTo(_actionSelector.GetControllerType()));

                Assert.That(
                    GetMethodName((ProductsController c) => c.GetPaged(1, 7)),
                    Is.EqualTo(_actionSelector.GetActionName()));
                
            }
            
            [Test]
            public void Route_Should_Post_Order_Action_IsInvoked()
            {
                // Arrange
                var request = new HttpRequestMessage(HttpMethod.Post, _dummyAddress + "api/orders/");

                // Act
                var _actionSelector = new ControllerActionSelector(_config, request);

                // Assert
                Assert.That(GetMethodName((OrdersController c) =>
                    c.PostOrder(new Order())), 
                    Is.EqualTo(_actionSelector.GetActionName()));
            }

            //[Test]
            //public void Route_Should_Post_Customer_Action_IsInvoked()
            //{
            //    // Arrange
            //    var request = new HttpRequestMessage(HttpMethod.Post, _dummyAddress + "api/customers/");

            //    // Act
            //    var _actionSelector = new ControllerActionSelector(_config, request);

            //    // Assert
            //    Assert.That(GetMethodName((CustomersController c) =>
            //        c.PostCustomer(new Customer())),
            //            Is.EqualTo(_actionSelector.GetActionName()));
            //}


            [Test]
            public void Route_Should_InvalidRoute_ThrowException()
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _dummyAddress + "/api/InvalidController/");

                var _actionSelector = new ControllerActionSelector(_config, request);

                Assert.Throws<HttpResponseException>(() => _actionSelector.GetActionName());
            }

        #endregion

        #region Helper methods

            public static string GetMethodName<T, U>(Expression<Func<T, U>> expression)
            {
                var method = expression.Body as MethodCallExpression;
                if (method != null)
                    return method.Method.Name;

                throw new ArgumentException("Expression is wrong");
            }
        
        #endregion
    }
}
