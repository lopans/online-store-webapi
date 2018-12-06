using Base.DAL;
using Data.Enums;

namespace Data.Entities.Store
{
    public class Category: BaseEntity, IFlexDisplayed
    {
        public string Title { get; set; }
        public virtual FileData Image { get; set; }
        public int? ImageID { get; set; }
        public string Color { get; set; }
    }
}
