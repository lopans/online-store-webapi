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
        public IUnitOfWork CreateUnitOfWork
        {
            get => uofw ?? new UnitOfWork(new DataContext());
        }
        public ClaimsPrincipal AppUser => HttpContext.Current.GetOwinContext().Authentication.User;
    }
}
