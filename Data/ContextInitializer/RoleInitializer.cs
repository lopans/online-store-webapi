using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Security.Entities;
using System.Linq;

namespace Data.ContextInitializer
{
    public class RoleInitializer : IContextInitializer
    {
        public void Initialize(DataContext context)
        {
            var store = new RoleStore<Role>(context);
            var manager = new RoleManager<Role>(store);
            if (!manager.Roles.Any())
            {
                manager.Create(new Role(Roles.Admin));
                manager.Create(new Role(Roles.Editor));
                manager.Create(new Role(Roles.Byuer));
                manager.Create(new Role(Roles.Public));
            }
        }
    }
}
