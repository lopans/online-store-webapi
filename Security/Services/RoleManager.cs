using Base;
using Microsoft.AspNet.Identity;
using Security.Entities;
using System.Linq;

namespace Security.Services
{
    public interface IRoleManager
    {
       IQueryable<Role> Roles { get; }

    }
    public class RoleManager : RoleManager<Role>, IRoleManager
    {
        public RoleManager(IRoleStore<Role> store, IDataContext dataContext) 
            : base(new RoleStore(dataContext))
        {
        }
    }
}
