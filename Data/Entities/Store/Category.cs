using Base;
using Base.DAL;
using Data.Entities.Core;
using Data.Enums;

namespace Data.Entities.Store
{
    public class Category: BaseEntity, IClientEntity
    {
        public string Title { get; set; }
        public virtual FileData Image { get; set; }
        public int? ImageID { get; set; }
        public string Color { get; set; }
    }
}
