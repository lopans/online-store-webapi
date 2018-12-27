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

namespace WebApi.Controllers
{
    [RoutePrefix("api/permissions")]
    public class PermissionsController : ApiControllerBase
    {
        private readonly IMappedBaseEntityService _mappedBaseEntityService;
        private readonly IAccessService _accessService;
        public PermissionsController(IAccessService accessService, IMappedBaseEntityService mappedBaseEntityService)
        {
            _mappedBaseEntityService = mappedBaseEntityService;
            _accessService = accessService;
        }
        [HttpGet]
        [Route("getList")]
        public async Task<IHttpActionResult> GetPermissionsList()
        {
            // TODO: доделать
            _accessService.ThrowIfNotInRole(Roles.Admin);
            var ret = (await _mappedBaseEntityService.GetEntitiesAsync()).Select(x => new
            {
                type = x.TypeName,
                roles = Roles.GetRolesList
            });
            return Ok(ret);
        }
    }
}