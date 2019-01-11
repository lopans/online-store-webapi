using Base.Exceptions;
using Data.Entities;
using Data.Services;
using Security.Services;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Base.Utils;
using Base.Services;
using Data.Services.Core;
using Data.Entities.Core;
using System.Linq;
using System.Data.Entity;

namespace WebApi.Controllers
{
    [RoutePrefix("api/permissions")]
    public class PermissionsController : ApiControllerBase
    {
        private readonly IMappedBaseEntityService _mappedBaseEntityService;
        private readonly IAccessService _accessService;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        public PermissionsController(IAccessService accessService, 
            IMappedBaseEntityService mappedBaseEntityService, 
            IUserManager userManager,
            IRoleManager roleManager)
        {
            _roleManager = roleManager;
            _mappedBaseEntityService = mappedBaseEntityService;
            _accessService = accessService;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("getList")]
        public async Task<IHttpActionResult> GetPermissionsList(string roleID)
        {
            _accessService.ThrowIfNotInRole(Roles.Admin);
            using(var uofw = CreateUnitOfWork)
            {
                return Ok (await _mappedBaseEntityService.GetEntityPermissionsForRole(uofw, roleID));

            }
        }

        [HttpGet]
        [Route("getRoles")]
        public async Task<IHttpActionResult> GetRoles()
        {
            return Ok(await _roleManager.Roles.Select(x => new { x.Id, x.Name }).ToListAsync());
        }
    }
}