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
using WebShopCase.API.Data;

namespace WebShopCase.Web
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // User a single instance of NorthwindEntities and CustomerManager per request
            app.CreatePerOwinContext(NorthwindEntities.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(10),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
