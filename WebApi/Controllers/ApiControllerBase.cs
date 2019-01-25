using Base.DAL;
using Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Security.Services;
using System.Collections;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApiControllerBase : ApiController
    {
        private IUnitOfWork uofw;
        private ISystemUnitOfWork suofw;
        public IUnitOfWork CreateUnitOfWork
        {
            get
            {
                if (uofw == null) uofw = new UnitOfWork(new DataContext());
                return uofw;
            }
        }

        public ISystemUnitOfWork CreateSystemUnitOfWork
        {
            get
            {
                if (suofw == null) suofw = new SystemUnitOfWork(new DataContext());
                return suofw;
            }
        }

        public async Task<IHttpActionResult> WrapListViewResult<T>(IEnumerable data, T type, IUnitOfWork uofw, IAccessService accessService)
        {
            var permissions = await accessService.GetPermissionsForType(uofw, type);
            return Ok(new
            {
                Data = data,
                Create = permissions.Create,
                Update = permissions.Update,
                Delete = permissions.Delete

            });
        }
       
        public ClaimsPrincipal AppUser => HttpContext.Current.GetOwinContext().Authentication.User;
    }
}
