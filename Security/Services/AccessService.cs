using Base.DAL;
using Base.Enums;
using Base.Services;
using Data.Entities.Core;
using Data.Services.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Security.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AppContext = Base.AppContext;

namespace Security.Services
{
    public class AccessService : IAccessService
    {
        private readonly IUserManager _userManager;
        public AccessService(IUserManager userManager)
        {
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
                .Where(x => userRoles.Contains(x.Role.Name) && 
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
    }
}
