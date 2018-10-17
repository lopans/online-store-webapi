using Base.Exceptions;
using Security.Services;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

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
        [HttpGet]
        [Route("register")]
        public async Task<IHttpActionResult> Register(string email, string password)
        {
            try
            {
                await _authenticationService.Register(email, password, UserManager);
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
            var aa = HttpContext.Current.GetOwinContext().Authentication.User;
            return Ok("done");
        }
    }
}