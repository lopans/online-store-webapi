using Microsoft.AspNet.Identity.EntityFramework;
using Security.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
