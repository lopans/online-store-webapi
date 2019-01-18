using Base.DAL;
using Data.ContextInitializer;
using Data.Entities;
using Data.Entities.Core;
using Data.Entities.Store;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Data
{
    public class DataContext : IdentityDbContext<User>
    {
        private BaseContextInitializer initializer;
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
            modelBuilder.Entity<SpecialPermission>();
            modelBuilder.Entity<RoleSpecialPermissions>();
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
            BaseContextInitializer initializer = new BaseContextInitializer();
            initializer.Initialize(context);
            base.Seed(context);
        }
    }
}
