using Base;
using Base.DAL;
using Base.Enums;
using Base.Services;
using Data.Entities.Core;
using Data.Entities.Store;
using Data.Services.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
