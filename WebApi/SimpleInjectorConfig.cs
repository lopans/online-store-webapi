using Base.Services;
using Data.Services;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Web.Http;

namespace WebApi
{
    public class SimpleInjectorConfig
    {
        public static Container Configure()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Register your types, for instance using the scoped lifestyle:
            container.Register(typeof(IBaseService<>), typeof(BaseService<>), Lifestyle.Singleton);
            container.Register<ITestObjectService, TestObjectService>(Lifestyle.Singleton);
            container.Register<IFileSystemService, FileSystemService>(Lifestyle.Singleton);


            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
            return container;
        }

    }
}
