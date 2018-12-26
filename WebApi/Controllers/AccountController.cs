using Base.Exceptions;
using Security;
using Security.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController: ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        public AccountController(IAuthenticationService authenticationService, IUserService userService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }
        [HttpGet]
        [Route("register")]
        public async Task<IHttpActionResult> Register(string email, string password)
        {
            try
            {
                await _authenticationService.Register(email, password);
                return Ok("registered successfully");
            }
            catch(RegisterFailedException rex)
            {
                throw rex;
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