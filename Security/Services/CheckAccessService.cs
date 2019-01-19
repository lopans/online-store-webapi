using Base.DAL;
using Base.Enums;
using Base.Services;
using Security.Entities;
using Security.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Security.Services
{
    public class CheckAccessService: ICheckAccessService
    {
        private readonly IUserManager _userManager;
        public CheckAccessService(IUserManager userManager)
        {
            _userManager = userManager;
        }
        public void ThrowIfAccessDenied(IUnitOfWork uofw, AccessModifier permission, Type entityType)
        {
            if (uofw is ISystemUnitOfWork)
                return;
            var userID = Base.AppContext.UserID;
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
            if (!hasPermission)
                throw new SecurityException(entityType, permission);
        }
    }
}
