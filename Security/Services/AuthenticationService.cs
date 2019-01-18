using Base.Exceptions;
using Data.Entities.Core;
using Data.Services.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Services
{
    public interface IAuthenticationService
    {
        Task Register(string login, string password);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserManager _userManager;

        public AuthenticationService(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task Register(string email, string password)
        {
            var user = new User()
            {
                Email = email,
                UserName = email,
            };
            IdentityResult res = await _userManager.CreateAsync(user, password);
            if (res.Succeeded)
            {
                await _userManager.AddToRoleAsync(user.Id, Roles.Byuer);
                return;
            }

            else throw new RegisterFailedException(res.Errors.First());
        }
    }
}
