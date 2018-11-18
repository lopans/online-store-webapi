using Microsoft.AspNet.Identity;
using Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Services
{
    public interface IUserService
    {
        Task<IEnumerable<string>> GetUserRoles(UserManager<User> manager, Guid? userID);
    }
    public class UserService: IUserService
    {
        public async Task<IEnumerable<string>> GetUserRoles(UserManager<User> manager, Guid? userID)
        {
            if (!userID.HasValue)
                return Enumerable.Empty<string>();
            return await manager.GetRolesAsync(userID.ToString());
        }
    }
}
