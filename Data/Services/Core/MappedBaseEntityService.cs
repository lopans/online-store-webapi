using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Entities;
using Base.Services;

namespace Data.Services.Core
{
    public interface IMappedBaseEntityService
    {
        Task<IEnumerable<MappedBaseEntity>> GetEntitiesAsync();
    }
    public class MappedBaseEntityService : IMappedBaseEntityService
    {
        private readonly IAccessService _accessService;
        public async Task<IEnumerable<MappedBaseEntity>> GetEntitiesAsync()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(BaseEntity)) && !x.IsAbstract)
                    .Select(x => new MappedBaseEntity() { TypeName = x.FullName })
                );
        }
    }
}
