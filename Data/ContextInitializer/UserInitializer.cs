using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Security.Entities;
using System.Linq;

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
