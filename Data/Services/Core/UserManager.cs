using Base.Identity.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Core
{
    public interface IUserManager
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(string userId, string role);
        Task<IList<string>> GetRolesAsync(string userId);

    }
    public class UserManager : UserManager<User>, IUserManager
    {
        public UserManager(IUserStore<User> store) : base(new UserStore())
        {
        }
    }
}
