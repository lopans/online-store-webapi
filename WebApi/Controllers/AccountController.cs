using Base;
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
        private readonly IUserManager _userManager;
        private readonly IAccessService _accessService;
        public AccountController(IAuthenticationService authenticationService, IUserManager userManager, IAccessService accessService)
        {
            _accessService = accessService;
            _userManager = userManager;
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
        [Route("getUserInfo")]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            var info = await _userManager.FindByIdAsync(AppContext.UserID);
            using (var uofw = CreateSystemUnitOfWork)
            {
                return Ok(new
                {
                    SpecialPermissions = await _accessService.GetSpecialsForRoles(uofw, info.Roles.Select(x => x.RoleId))
                });
            }

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