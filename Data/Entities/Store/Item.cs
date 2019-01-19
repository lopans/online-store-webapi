using Base;
using Base.DAL;
using System.Collections.Generic;

namespace Data.Entities.Store
{
    public class Item: BaseEntity, IClientEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
