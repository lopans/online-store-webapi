using Base.Exceptions;
using Security.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController: ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            try
            {
                await _authenticationService.Register(model.Email, model.Password);
                return Ok("registered successfully");
            }
            catch(RegisterFailedException rex)
            {
                return InternalServerError(rex);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("test")]
        public async Task<IHttpActionResult> Test()
        {
            var aa = HttpContext.Current.GetOwinContext().Authentication.User.Identity;
            return Ok("done");
        }

        [HttpPost]
        [Route("tinyProfile")]
        [Authorize]
        public async Task<IHttpActionResult> GetTinyProfile()
        {
            // TODO: UserManagerWrap
            return Ok(new
            {
                roles = Enumerable.Empty<string>()// await _userService.GetUserRoles(UserManager, AppUser.GetUserID())
            });
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<IHttpActionResult> Logout()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut();
            return Ok();
        }
    }
}