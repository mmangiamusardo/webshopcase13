using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using Newtonsoft.Json.Serialization;
using Autofac;
using Autofac.Integration.WebApi;

using WebShopCase.API.Core;
using WebShopCase.API.Core.Controllers;
using WebShopCase.API.Service;
using WebShopCase.API.Data.Repositories;
using WebShopCase.API.Data.Infrastructure;

namespace WebShopCase.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IAssembliesResolver), 
                new CustomAssembliesResolver());

            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Autofac configuration
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(ProductsController).Assembly);

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            //Repositories
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(ProductService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Remove default xml formatter
            var json = config.Formatters.JsonFormatter;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            
        }
    }
}
