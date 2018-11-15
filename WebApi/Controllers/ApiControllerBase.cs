using Base.DAL;
using Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Security.Entities;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApiControllerBase : ApiController
    {
        private IUnitOfWork uofw;
        private IUnitOfWork suofw;
        public IUnitOfWork CreateUnitOfWork
        {
            get => uofw ?? new UnitOfWork(new DataContext());
        }
        public ClaimsPrincipal AppUser => HttpContext.Current.GetOwinContext().Authentication.User;
        public IUnitOfWork CreateSecurityUnitOfWork
        {
            get => suofw ?? new UnitOfWork(new Security.SecurityContext());
        }

        public UserManager<User> UserManager
        {
            get => HttpContext.Current.GetOwinContext().GetUserManager<UserManager<User>>() ??
                new UserManager<User>(new UserStore<User>(new Security.SecurityContext()));
        }
    }
}
