using Base;
using Base.DAL;
using Base.Services;
using Base.Services.Media;
using Data;
using Data.Services;
using Microsoft.AspNet.Identity;
using Security.Entities;
using Security.Services;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Data.Entity;
using System.Web.Http;

namespace WebApi
{
    public class SimpleInjectorConfig
    {
        public static Container Configure()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            //container.RegisterInstance(container);
            // Register your types, for instance using the scoped lifestyle:
            #region DATA
            container.Register(typeof(IBaseService<>), typeof(BaseService<>));
            container.Register<ITestObjectService, TestObjectService>();
            container.Register<IFileSystemService, FileSystemService>();
            #endregion
            container.Register<IAuthenticationService, AuthenticationService>();
            container.Register<ICheckAccessService, CheckAccessService>();
            container.Register<IAccessService, AccessService>();
            container.Register<IDataContext, DataContext>(Lifestyle.Scoped);
            container.Register<DbContext, DataContext>(Lifestyle.Scoped);
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
            container.Register<ISystemUnitOfWork, SystemUnitOfWork>(Lifestyle.Scoped);
            container.Register<IUserManager, UserManager>(Lifestyle.Scoped);
            container.Register<IRoleManager, RoleManager>(Lifestyle.Scoped);
            container.Register<IUserStore<User>, UserStore>(Lifestyle.Scoped);
            container.Register<IRoleStore<Role>, RoleStore>(Lifestyle.Scoped);
            container.Register<IUserStore, UserStore>(Lifestyle.Scoped);
            container.Register<IRoleStore, RoleStore>(Lifestyle.Scoped);
            
            container.Register<IMappedBaseEntityService, MappedBaseEntityService>();

            

            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
            return container;
        }

    }
}
