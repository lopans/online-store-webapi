using Base;
using Microsoft.AspNet.Identity;
using Security.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Security.Services
{
    public interface IUserManager
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(string userId, string role);
        Task<IList<string>> GetRolesAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<User> FindByIdAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);

    }
    public class UserManager : UserManager<User>, IUserManager
    {
        public UserManager(IUserStore<User> store, IDataContext _dataContext) 
            : base(new UserStore(_dataContext))
        {
        }
    }
}
