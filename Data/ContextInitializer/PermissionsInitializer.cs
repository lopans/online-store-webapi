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
    public class PermissionsInitializer : IContextInitializer
    {

        public void Initialize(DataContext context)
        {
            var store = new RoleStore<Role>(context);
            var manager = new RoleManager<Role>(store);
            var uofw = new SystemUnitOfWork(context);
            
            var repo = uofw.GetRepository<AccessLevel>();
            if (repo.All().Any())
                return;
            InitBuyer(repo, manager);
            InitEditor(repo, manager);
            uofw.SaveChanges();
        }

        private void InitBuyer(IRepository<AccessLevel> repo, RoleManager<Role> manager)
        {
        }

        private void InitEditor(IRepository<AccessLevel> repo, RoleManager<Role> manager)
        {
            var editorRoleID = manager.Roles.Where(x => x.Name == Roles.Editor).Single().Id;
            foreach (var al in ConstructAccessLevel<Category>(editorRoleID, new AccessModifier[] { AccessModifier.Create, AccessModifier.Update }))
                repo.Create(al);

            foreach (var al in ConstructAccessLevel<SubCategory>(editorRoleID, new AccessModifier[] { AccessModifier.Create, AccessModifier.Update }))
                repo.Create(al);

            foreach (var al in ConstructAccessLevel<Item>(editorRoleID, new AccessModifier[] { AccessModifier.Create, AccessModifier.Update }))
                repo.Create(al);

            foreach (var al in ConstructAccessLevel<SaleItem>(editorRoleID, new AccessModifier[] { AccessModifier.Create, AccessModifier.Update }))
                repo.Create(al);
        }
        private IEnumerable<AccessLevel> ConstructAccessLevel<T>(string roleID, AccessModifier[] modifiers)
            where T : BaseEntity
        {
            foreach (var item in modifiers)
            {
                yield return new AccessLevel()
                {
                    RoleID = roleID,
                    AccessModifier = item,
                    Entity = new Base.Entities.MappedBaseEntity()
                    {
                        TypeName = typeof(T).FullName
                    }
                };
            }
        }
    }
}
