using Base.DAL;
using Base.Identity.Entities;
using Data.Entities;
using Data.Entities.Store;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Security.Entities;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext()
            : base("DataContext")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DataContext>(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TestObject>();
            modelBuilder.Entity<TestObject1>();

            modelBuilder.Entity<Category>();
            modelBuilder.Entity<FileData>();
            modelBuilder.Entity<SubCategory>();
            modelBuilder.Entity<Item>();
            modelBuilder.Entity<SaleItem>();
            modelBuilder.Entity<AccessLevel>();
            //modelBuilder.Entity<SubCategoryItem>();

            base.OnModelCreating(modelBuilder);
        }

        
    }
    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Data.DataContext";
        }
        protected override void Seed(DataContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            if (!manager.Roles.Any())
            {
                manager.Create(new IdentityRole(Roles.Admin));
                manager.Create(new IdentityRole(Roles.Editor));
                manager.Create(new IdentityRole(Roles.Byuer));
            }

            var ustore = new UserStore<User>(context);
            var umanager = new UserManager<User>(ustore);
            if (!umanager.Users.Any() || !umanager.Users.Any(x => x.Email == "admin"))
            {
                User admin = new User();
                admin.Email = admin.UserName = "admin";
                umanager.Create(admin, "111111");
                umanager.AddToRole(admin.Id, Roles.Admin);
            }
            base.Seed(context);
        }
    }
}
