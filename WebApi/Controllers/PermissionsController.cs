using Base.Services;
using Security.Entities;
using Security.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/permissions")]
    [Authorize]
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
                return Ok (await _accessService.GetEntityPermissionsForRole(uofw, roleID));
            }
        }

        [HttpGet]
        [Route("getSpecialList")]
        public async Task<IHttpActionResult> GetSpecialPermissionsList(string roleID)
        {
            _accessService.ThrowIfNotInRole(Roles.Admin);
            using (var uofw = CreateUnitOfWork)
            {
                return Ok(await _accessService.GetRoleSpecialPermissions(uofw, roleID));
            }
        }

        [HttpGet]
        [Route("getRoles")]
        public async Task<IHttpActionResult> GetRoles()
        {
            return Ok(await _roleManager.Roles.Select(x => new { x.Id, x.Name }).ToListAsync());
        }

        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> Update(PermissionUpdateModel model)
        {
            using(var uofw = CreateUnitOfWork)
            {
                try
                {
                    await _accessService.UpdatePermissionForRole(uofw, model.EntityType, model.RoleID, model.Permission, model.IsEnabled);
                    return Ok();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        [HttpPost]
        [Route("updateSpecial")]
        public async Task<IHttpActionResult> UpdateSpecial(SpecialPermissionUpdateModel model)
        {
            using (var uofw = CreateUnitOfWork)
            {
                try
                {
                    await _accessService.UpdateSpecialPermissionForRole(uofw, model.RoleID, model.PermissionID, model.IsEnabled);
                    return Ok();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}