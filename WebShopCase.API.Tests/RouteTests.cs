using System.Net.Http;
using System.Web.Http;
using MvcRouteTester.Test.Assertions;

using WebShopCase.API.Data;
using WebShopCase.API.Data.Infrastructure;
using WebShopCase.API.Data.Repositories;
using WebShopCase.API.Domain;
using WebShopCase.API.Service;
using WebShopCase.API.Core.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
    [TestFixture]
    public class FluentExtensionsTests
    {
        private HttpConfiguration config;

        [SetUp]
        public void MakeRouteTable()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());

            config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
              name: "PagedItemsApi",
              routeTemplate: "api/{controller}/page/{pageIndex}/{pageSize}",
              defaults: new { action = "GetPaged" }
          );
        }

        [TearDown]
        public void TearDown()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());
        }

        [Test]
        public void Route_Should_Controller_GetProductById()
        {
            config.ShouldMap("/api/products/1")
                .To<ProductsController>(HttpMethod.Get, x => x.GetById(1));
        }

        //[Test]
        //public void ShouldMapToFailsWithWrongRoute()
        //{
        //    var assertEngine = new FakeAssertEngine();
        //    RouteAssert.UseAssertEngine(assertEngine);

        //    config.ShouldMap("/api/missing/32/foo").To<CustomerController>(HttpMethod.Get, x => x.Get(32));

        //    Assert.That(assertEngine.FailCount, Is.EqualTo(4));
        //    Assert.That(assertEngine.Messages[0], Is.EqualTo("No route matched url 'http://site.com/api/missing/32/foo'"));
        //}

        [Test]
        public void TestNoRouteForMethod()
        {
            config.ShouldMap("/api/products/32").ToNoMethod<ProductsController>(HttpMethod.Post);
        }

        //[Test]
        //public void ShouldMapToNoMethodFailsOnValidRoute()
        //{
        //    var assertEngine = new FakeAssertEngine();
        //    RouteAssert.UseAssertEngine(assertEngine);

        //    config.ShouldMap("/api/customer/32").ToNoMethod<CustomerController>(HttpMethod.Get);

        //    Assert.That(assertEngine.FailCount, Is.EqualTo(1));
        //    Assert.That(assertEngine.Messages[0], Is.EqualTo("Method GET is allowed on url '/api/customer/32'"));
        //}

        [Test]
        public void TestNoRoute()
        {
            config.ShouldMap("/foo/products/32").ToNoRoute();
        }

        //[Test]
        //public void ShouldMapToNoRouteFailsOnValidRoute()
        //{
        //    var assertEngine = new FakeAssertEngine();
        //    RouteAssert.UseAssertEngine(assertEngine);

        //    config.ShouldMap("/api/customer/32").ToNoRoute();

        //    Assert.That(assertEngine.FailCount, Is.EqualTo(1));
        //    Assert.That(assertEngine.Messages[0], Is.EqualTo("Found a route for url '/api/customer/32'"));
        //}
    }
}
