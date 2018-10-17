using Base.DAL;
using Data.Entities;
using Data.Entities.Store;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Data
{
    public class DataContext : DbContext
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
            //modelBuilder.Entity<SubCategoryItem>();
        }

        //public virtual DbSet<TestObject> TestObjects { get; set; }
    }
    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Data.DataContext";
        }
    }
}
