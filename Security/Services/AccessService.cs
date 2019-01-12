using Base.DAL;
using Base.Enums;
using Base.Models;
using Base.Services;
using Data.Entities.Core;
using Data.Services.Core;
using Security.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AppContext = Base.AppContext;

namespace Security.Services
{
    public class AccessService : IAccessService
    {
        private readonly IUserManager _userManager;
        private readonly IMappedBaseEntityService _mappedBaseEntityService;
        public AccessService(IUserManager userManager, IMappedBaseEntityService mappedBaseEntityService)
        { 
            _mappedBaseEntityService = mappedBaseEntityService;
            _userManager = userManager;
        }
        public void ThrowIfAccessDenied(IUnitOfWork uofw, AccessModifier permission, Type entityType)
        {
            if (uofw is ISystemUnitOfWork)
                return;
            var userID = AppContext.UserID;
            var userRoles = userID != null ?
                _userManager.GetRolesAsync(userID).Result :
                new List<string> { Roles.Public };
            if (userRoles.Contains(Roles.Admin))
                return;

            var hasPermission = uofw.GetRepository<AccessLevel>().All()
                .Where(x => !x.Hidden && userRoles.Contains(x.Role.Name) && 
                x.AccessModifier == permission && 
                x.Entity.TypeName == entityType.FullName)
                .Any();
            if(!hasPermission)
                throw new SecurityException(entityType, permission);
        }

        public void ThrowIfNotInRole(string role)
        {
            var userID = AppContext.UserID;
            var userRoles = userID != null ?
                _userManager.GetRolesAsync(userID).Result :
                new List<string> { Roles.Public };

            if(!userRoles.Contains(role))
                throw new SecurityException("You got not enough role to perform this action");
        }

        public async Task<IEnumerable<EntityPermissionSet>> GetEntityPermissionsForRole(IUnitOfWork uofw, string roleID)
        {
            var rolePermissions = await uofw.GetRepository<AccessLevel>().All()
                .Where(x => !x.Hidden && x.RoleID == roleID)
                .Select(x => new
                {
                    x.Entity.TypeName,
                    x.AccessModifier
                })
                .ToListAsync();
            var allEntities = await _mappedBaseEntityService.GetEntitiesAsync();
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

        public async Task UpdatePermissionForRole(IUnitOfWork uofw, string entityType, string roleID, AccessModifier accessModifier, bool isEnabled)
        {
            var repo = uofw.GetRepository<AccessLevel>();
            var entry = await repo.All()
                .Where(x => x.RoleID == roleID && x.AccessModifier == accessModifier && x.Entity.TypeName == entityType)
                .Select(x => new { x.ID })
                .FirstOrDefaultAsync();
            if(entry != null)
            {
                if (!isEnabled)
                    repo.Delete(x => x.ID == entry.ID);
            }
            else
            {
                repo.Create(new AccessLevel()
                {
                    AccessModifier = accessModifier,
                    RoleID = roleID,
                    Entity = new Base.Entities.MappedBaseEntity()
                    {
                        TypeName = entityType
                    }
                });
            }
            await uofw.SaveChangesAsync();
        }
    }
}
