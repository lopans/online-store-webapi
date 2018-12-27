using Base.Identity.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Security.OAuthServer;
using SimpleInjector.Integration.WebApi;
using System;
using System.Data.Entity.Migrations;
using System.Web.Http;


[assembly: OwinStartup(typeof(WebApi.Startup))]
namespace WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Initialize container
            var container = SimpleInjectorConfig.Configure(); 

            HttpConfiguration config = new HttpConfiguration
            {
                DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container)
            };
            app.CreatePerOwinContext(() => new Data.DataContext());
            app.CreatePerOwinContext<UserManager<User>>(CreateManager);

            // token generation
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AuthorizeEndpointPath = new PathString("/auth"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                RefreshTokenProvider = new ApplicationRefreshTokenProvider(),
                Provider = new OAuthServerProvider(),
            });

            // token consumption
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseWebApi(WebApiConfig.Register(config));
            MigrateDB();
        }

        private static UserManager<User> CreateManager(IdentityFactoryOptions<UserManager<User>> options, IOwinContext context)
        {
            var userStore = new UserStore<User>(context.Get<Data.DataContext>());
            var owinManager = new UserManager<User>(userStore);
            return owinManager;
        }

        static void MigrateDB()
        {
            //var securityMigratorConfig = new Security.Configuration();
            //var securityDbMigrator = new DbMigrator(securityMigratorConfig);
            //securityDbMigrator.Update();

            var migratorConfig = new Data.Configuration();
            var dbMigrator = new DbMigrator(migratorConfig);
            dbMigrator.Update();
        }
    }


}
