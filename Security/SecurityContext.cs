using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Security.Entities;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Security
{
    public class SecurityContext: IdentityDbContext<User>
    {
        public SecurityContext():base("DataContext")
        {

        }

    }

    public sealed class Configuration : DbMigrationsConfiguration<SecurityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Security.SecurityContext";
        }
        protected override void Seed(SecurityContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            if (!manager.Roles.Any())
            {
                manager.Create(new IdentityRole(Role.Admin));
                manager.Create(new IdentityRole(Role.Editor));
                manager.Create(new IdentityRole(Role.Byuer));
            }

            var ustore = new UserStore<User>(context);
            var umanager = new UserManager<User>(ustore);
            if (!umanager.Users.Any() || !umanager.Users.Any(x => x.Email == "admin"))
            {
                User admin = new User();
                admin.Email = admin.UserName = "admin";
                umanager.Create(admin, "111111");
                umanager.AddToRole(admin.Id, Role.Admin);
            }
            base.Seed(context);
        }
    }
}
