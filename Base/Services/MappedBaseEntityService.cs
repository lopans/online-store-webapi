using Base.DAL;
using Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Services
{
    public interface IMappedBaseEntityService
    {
        Task<IEnumerable<MappedBaseEntity>> GetEntitiesAsync();
    }
    public class MappedBaseEntityService : IMappedBaseEntityService
    {
        public MappedBaseEntityService()
        {
        }
        public async Task<IEnumerable<MappedBaseEntity>> GetEntitiesAsync()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(BaseEntity)) && !x.IsAbstract && x.GetInterfaces().Contains(typeof(IClientEntity)))
                    .Select(x => new MappedBaseEntity() { TypeName = x.FullName })
                );
        }
    }
}
