using Data.Entities;
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
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<TestObject> TestObjects { get; set; }
    }
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Data.DataContext";
        }
    }
}
