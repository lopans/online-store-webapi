using Base.DAL;
using Base.Enums;
using Base.Services;
using Security.DTO;
using Security.Entities;
using Security.Exceptions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AppContext = Base.AppContext;

namespace Security.Services
{
    public interface IAccessService
    {
        void ThrowIfNotInRole(string role);
        Task<EntityPermissionSet> GetPermissionsForType<T>(IUnitOfWork uofw, T type, IEnumerable<string> roleIDs = null);

        Task<IEnumerable<EntityPermissionSet>> GetEntityPermissionsForRole(IUnitOfWork uofw, string roleID);
        Task<IEnumerable<RoleSpecialPermissionDTO>> GetRoleSpecialPermissions(IUnitOfWork uofw, string roleID);
        Task UpdatePermissionForRole(IUnitOfWork uofw, string entityType, string roleID, AccessModifier accessModifier, bool isEnabled);
        Task UpdateSpecialPermissionForRole(IUnitOfWork uofw, string roleID, int specialPermissionID, bool isEnabled);
        Task<IEnumerable<string>> GetSpecialsForRoles(IUnitOfWork uofw, IEnumerable<string> roles);
    }
    public class AccessService : IAccessService
    {
        private readonly IUserManager _userManager;
        private readonly IMappedBaseEntityService _mappedBaseEntityService;
        public AccessService(IUserManager userManager, IMappedBaseEntityService mappedBaseEntityService)
        { 
            _mappedBaseEntityService = mappedBaseEntityService;
            _userManager = userManager;
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

        public async Task<IEnumerable<RoleSpecialPermissionDTO>> GetRoleSpecialPermissions(IUnitOfWork uofw, string roleID)
        {
            var rolePermissionIDs = await uofw.GetRepository<RoleSpecialPermissions>().All()
                .Where(x => !x.Hidden && x.RoleID == roleID)
                .Select(x => x.SpecialPermissionID)
                .ToListAsync();

            var allSpecial = uofw.GetRepository<SpecialPermission>().All()
                .Where(x => !x.Hidden).
                Select(x => new
                {
                    x.ID,
                    x.Title
                });
            var ret = new List<RoleSpecialPermissionDTO>();
            foreach (var item in allSpecial)
            {
                var el = new RoleSpecialPermissionDTO()
                {
                    PermissionID = item.ID,
                    PermissionTitle = item.Title,
                    IsEnabled = rolePermissionIDs.Contains(item.ID)
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

        public async Task UpdateSpecialPermissionForRole(IUnitOfWork uofw, string roleID, int specialPermissionID, bool isEnabled)
        {
            var repo = uofw.GetRepository<RoleSpecialPermissions>();
            var entry = await repo.All()
                .Where(x => x.RoleID == roleID && x.SpecialPermissionID == specialPermissionID)
                .Select(x => new { x.ID })
                .FirstOrDefaultAsync();
            if (entry != null)
            {
                if (!isEnabled)
                    repo.Delete(x => x.ID == entry.ID);
            }
            else
            {
                repo.Create(new RoleSpecialPermissions()
                {
                    SpecialPermissionID = specialPermissionID,
                    RoleID = roleID,
                });
            }
            await uofw.SaveChangesAsync();
        }
        public async Task<IEnumerable<string>> GetSpecialsForRoles(IUnitOfWork uofw, IEnumerable<string> roles)
        {
            return await uofw.GetRepository<RoleSpecialPermissions>().All()
                .Where(x => !x.Hidden && roles.Contains(x.RoleID))
                .Select(x => x.SpecialPermission.Title)
                .Distinct()
                .ToListAsync();
        }

        public async Task<EntityPermissionSet> GetPermissionsForType<T>(IUnitOfWork uofw, T type, IEnumerable<string> roleIDs = null)
        {
            var typeName = typeof(T).FullName;

            if (AppContext.UserID == null && roleIDs == null)
                return EntityPermissionSet.FromReadOnly(typeName);
            else if (roleIDs == null && await _userManager.IsInRoleAsync(AppContext.UserID, Roles.Admin))
                return EntityPermissionSet.FromAll(typeName);

            roleIDs = roleIDs ?? await _userManager.GetRolesAsync(AppContext.UserID);

            var accessModifiers = await uofw.GetRepository<AccessLevel>().All()
                .Where(x => roleIDs.Contains(x.RoleID) && x.Entity.TypeName == typeName)
                .Select(x => new { x.AccessModifier })
                .ToListAsync();
            return new EntityPermissionSet()
            {
                Create = accessModifiers.Any(x => x.AccessModifier == AccessModifier.Create),
                Update = accessModifiers.Any(x => x.AccessModifier == AccessModifier.Update),
                Delete = accessModifiers.Any(x => x.AccessModifier == AccessModifier.Delete),
                Read = accessModifiers.Any(x => x.AccessModifier == AccessModifier.Read),
                EntityType = typeName
            };
        }
    }
}
