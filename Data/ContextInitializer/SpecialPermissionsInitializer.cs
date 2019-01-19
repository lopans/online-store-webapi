using Base.DAL;
using Security.Entities;
using System.Linq;

namespace Data.ContextInitializer
{
    public class SpecialPermissionsInitializer : IContextInitializer
    {

        public void Initialize(DataContext context)
        {
            var uofw = new SystemUnitOfWork(context);
            
            var repo = uofw.GetRepository<SpecialPermission>();
            if (repo.All().Any())
                return;
            repo.Create(new SpecialPermission() { Title = "Set permissions" });
            uofw.SaveChanges();
        }
    }
}
