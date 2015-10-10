using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

using WebShopCase.API.Core;
using WebShopCase.API.Core.Controllers;
using WebShopCase.API.Service;
using WebShopCase.API.Data.Repositories;
using WebShopCase.API.Data.Infrastructure;

namespace WebShopCase.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "SessionsRoute",
                routeTemplate: "api/sessions/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ).RouteHandler = new SessionEnabledHttpControllerRouteHandler(); ;

            // Web API Stateless Route Configurations
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapHttpRoute(
               name: "PagedItemsApiWithParams",
               routeTemplate: "api/{controller}/page/{pageIndex}/{pageSize}",
               defaults: new { 
                   action = "GetPaged"
               }
           );

            routes.MapHttpRoute(
               name: "PagedItemsApiNoParams",
               routeTemplate: "api/{controller}/page",
               defaults: new
               {
                   action = "GetPaged"
               }
            );
            
        }
    }
}
