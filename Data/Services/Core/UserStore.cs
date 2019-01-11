using Base.Identity.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Core
{
    public interface IUserStore: IUserStore<User>
    {

    }
    public class UserStore: UserStore<User>, IUserStore
    {
        public UserStore(): base(new DataContext())
        {

        }
    }


    public interface IRoleStore : IRoleStore<IdentityRole>
    {

    }
    public class RoleStore : RoleStore<IdentityRole>, IRoleStore
    {
        public RoleStore() : base(new DataContext())
        {

        }
    }
}
