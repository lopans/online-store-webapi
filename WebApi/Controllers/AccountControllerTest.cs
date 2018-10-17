using Base.Exceptions;
using Security.Services;
using System.Threading.Tasks;
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
        [Route("test")]
        public async Task<IHttpActionResult> Test()
        {
            return Ok("done");
        }
    }
}