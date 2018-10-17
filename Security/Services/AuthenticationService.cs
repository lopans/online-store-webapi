using Base.Exceptions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Services
{
    public interface IAuthenticationService
    {
        Task Register(string login, string password, UserManager<User> manager);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserStore<User> _userStore;
        private readonly UserManager<User> _userManager;

        public AuthenticationService()
        {
            _userStore = new UserStore<User>();
            _userManager = new UserManager<User>(_userStore);
        }

        public async Task Register(string email, string password, UserManager<User> manager)
        {
            var user = new User()
            {
                Email = email,
                UserName = email
            };
            IdentityResult res = await manager.CreateAsync(user, password);
            if (res.Succeeded)
                return;

            else throw new RegisterFailedException(res.Errors.First());
        }
    }
}
