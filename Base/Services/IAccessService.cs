using Base.DAL;
using Base.Enums;
using Base.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.Services
{
    public interface IAccessService
    {
        void ThrowIfAccessDenied(IUnitOfWork uofw, AccessModifier permission, Type entityType);
        Task UpdatePermissionForRole(IUnitOfWork uofw, string entityType, string roleID, AccessModifier accessModifier, bool isEnabled);
        Task<IEnumerable<EntityPermissionSet>> GetEntityPermissionsForRole(IUnitOfWork uofw, string roleID);
        void ThrowIfNotInRole(string role);
    }
}
