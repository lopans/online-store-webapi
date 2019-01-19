using Base;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Security.Entities;
using System.Data.Entity;

namespace Security.Services
{
    public interface IUserStore: IUserStore<User>
    {

    }
    public class UserStore: UserStore<User>, IUserStore
    {

        public UserStore(IDataContext dataContext) 
            : base((DbContext)dataContext)
        {

        }
    }


    public interface IRoleStore : IRoleStore<Role>
    {

    }
    public class RoleStore : RoleStore<Role>, IRoleStore
    {
        public RoleStore(IDataContext dataContext) 
            : base((DbContext)dataContext)
        {

        }
    }
}
