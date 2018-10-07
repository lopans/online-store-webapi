using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //var container = new Container();
            //container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            //// Register your types, for instance using the scoped lifestyle:
            //container.Register<ITestObjectService, TestObjectService>(Lifestyle.Scoped);

            //// This is an extension method from the integration package.
            //container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            //container.Verify();

            //GlobalConfiguration.Configuration.DependencyResolver =
            //    new SimpleInjectorWebApiDependencyResolver(container);


            AreaRegistration.RegisterAllAreas();
            //SimpleInjectorConfig.Configure();
            //GlobalConfiguration.Configure(Startup.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
