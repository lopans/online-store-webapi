using Base;
using Base.DAL;

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
