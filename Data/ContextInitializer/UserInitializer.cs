using Data.Entities.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ContextInitializer
{
    public class UserInitializer : IContextInitializer
    {
        public void Initialize(DataContext context)
        {
            var ustore = new UserStore<User>(context);
            var umanager = new UserManager<User>(ustore);
            if (!umanager.Users.Any() || !umanager.Users.Any(x => x.Email == "admin"))
            {
                User admin = new User();
                admin.Email = admin.UserName = "admin";
                umanager.Create(admin, "111111");
                umanager.AddToRole(admin.Id, Roles.Admin);
            }
        }
    }
}
