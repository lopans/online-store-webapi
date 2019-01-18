using Data.Entities.Core;
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
       IQueryable<Role> Roles { get; }

    }
    public class RoleManager : RoleManager<Role>, IRoleManager
    {
        public RoleManager(IRoleStore<Role> store) : base(new RoleStore())
        {
        }
    }
}
