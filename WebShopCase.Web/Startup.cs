using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using Autofac;
using Autofac.Integration.WebApi;

using WebShopCase.API.Core;
using WebShopCase.API.Core.Controllers;
using WebShopCase.API.Service;
using WebShopCase.API.Data.Repositories;
using WebShopCase.API.Data.Infrastructure;

[assembly: OwinStartup(typeof(WebShopCase.Web.Startup))]

namespace WebShopCase.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}