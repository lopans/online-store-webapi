using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Core
{
    public interface IRoleManager
    {
       IQueryable<IdentityRole> Roles { get; }

    }
    public class RoleManager : RoleManager<IdentityRole>, IRoleManager
    {
        public RoleManager(IRoleStore<IdentityRole> store) : base(new RoleStore())
        {
        }
    }
}
