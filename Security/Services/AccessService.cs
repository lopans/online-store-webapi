using Base.DAL;
using Base.Enums;
using Base.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Security.Entities;
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
        private readonly IAuthenticationService _authenticationService;
        public AccessService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public void ThrowIfAccessDenied(IUnitOfWork uofw, AccessModifier permission, Type entityType)
        {
            if (uofw is ISystemUnitOfWork)
                return;
            var userID = AppContext.UserID;
            // сделать обертку для манагера, просунуть ему datacontext. Иначе он не знает строки подключения
            var userRoles = userID != null ?
                _authenticationService.GetUserManager().GetRoles(userID) :
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
    }
}
