using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;

using Microsoft.Owin;
using Owin;
using Autofac;
using Autofac.Integration.WebApi;

using WebShopCase.API.Core;
using WebShopCase.API.Core.Controllers;
using WebShopCase.API.Service;
using WebShopCase.API.Data;
using WebShopCase.API.Data.Repositories;
using WebShopCase.API.Data.Infrastructure;

namespace WebShopCase.API.Tests.Hosting
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            //config.MessageHandlers.Add(new HeaderAppenderHandler());
            //config.MessageHandlers.Add(new EndRequestHandler());
            //config.Filters.Add(new ArticlesReversedFilter());
            
            //config.Services.Replace(typeof(IAssembliesResolver), new CustomAssembliesResolver());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MapHttpAttributeRoutes();

            // Autofac configuration
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(ProductsController).Assembly);

            // Unit of Work
            var _unitOfWork = new Mock<IUnitOfWork>();
            builder.RegisterInstance(_unitOfWork.Object).As<IUnitOfWork>();

            //Repositories
            var _prodRepository = new Mock<ProductRepository>();
            _prodRepository.Setup(x => x.GetAll()).Returns(
                    NorthwindInitializer.GetProducts()
                );
            builder.RegisterInstance(_prodRepository.Object).As<IProductRepository>();

            // Services
            builder.RegisterAssemblyTypes(typeof(ProductService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterInstance(new ProductService(_prodRepository.Object, _unitOfWork.Object));
            //builder.RegisterInstance(new BlogService(_blogsRepository.Object, _unitOfWork.Object));

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseWebApi(config);
        }
    }
}
