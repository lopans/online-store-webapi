using Base;
using Base.DAL;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities.Store
{
    public class Category: BaseEntity, IClientEntity
    {
        [MaxLength(255)]
        public string Title { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public virtual FileData Image { get; set; }
        public int? ImageID { get; set; }
        [MaxLength(255)]
        public string Color { get; set; }
    }
}
