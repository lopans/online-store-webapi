using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Entities;
using Base.Models;
using Base.Services;
using Data.Entities.Core;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Data.Services.Core
{
    public interface IMappedBaseEntityService
    {
        Task<IEnumerable<MappedBaseEntity>> GetEntitiesAsync();
        Task<IEnumerable<EntityPermissionSet>> GetEntityPermissionsForRole(IUnitOfWork uofw, string roleID);
    }
    public class MappedBaseEntityService : IMappedBaseEntityService
    {
        private readonly IAccessService _accessService;
        public MappedBaseEntityService(IAccessService accessService)
        {
            _accessService = accessService;
        }
        public async Task<IEnumerable<MappedBaseEntity>> GetEntitiesAsync()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(BaseEntity)) && !x.IsAbstract)
                    .Select(x => new MappedBaseEntity() { TypeName = x.FullName })
                );
        }

        public async Task<IEnumerable<EntityPermissionSet>> GetEntityPermissionsForRole(IUnitOfWork uofw, string roleID )
        {
            var rolePermissions = await uofw.GetRepository<AccessLevel>().All()
                .Where(x => !x.Hidden && x.RoleID == roleID)
                .Select(x => new
                {
                    x.Entity.TypeName,
                    x.AccessModifier
                })
                .ToListAsync();
            var allEntities = await GetEntitiesAsync();
            var ret = new List<EntityPermissionSet>();
            foreach (var item in allEntities)
            {
                var inRoleForType = rolePermissions.Where(x => x.TypeName == item.TypeName);
                var el = new EntityPermissionSet()
                {
                    EntityType = item.TypeName,
                    Create = inRoleForType.Any(x => x.AccessModifier == Base.Enums.AccessModifier.Create),
                    Read = inRoleForType.Any(x => x.AccessModifier == Base.Enums.AccessModifier.Read),
                    Update = inRoleForType.Any(x => x.AccessModifier == Base.Enums.AccessModifier.Update),
                    Delete = inRoleForType.Any(x => x.AccessModifier == Base.Enums.AccessModifier.Delete),
                };
                ret.Add(el);
            }
            return ret;
        }
    }
}
