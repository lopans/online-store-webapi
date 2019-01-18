using Base.DAL;
using Base.Enums;
using Data.DTO.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Services.Core
{
    public interface IAccessService
    {
        Task UpdatePermissionForRole(IUnitOfWork uofw, string entityType, string roleID, AccessModifier accessModifier, bool isEnabled);
        Task<IEnumerable<EntityPermissionSet>> GetEntityPermissionsForRole(IUnitOfWork uofw, string roleID);
        Task<IEnumerable<RoleSpecialPermissionDTO>> GetRoleSpecialPermissions(IUnitOfWork uofw, string roleID);
        Task UpdateSpecialPermissionForRole(IUnitOfWork uofw, string roleID, int specialPermissionID, bool isEnabled);
        void ThrowIfNotInRole(string role);
    }
}
