using Base;
using Base.DAL;
using Data.Entities.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Store
{
    public class SaleItem: BaseEntity, IClientEntity
    {
        public int ItemID { get; set; }
        public virtual Item Item { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public double NewPrice { get; set; }
        [NotMapped]
        public int Percent { get { return Item.Price > 0 ? (int)Math.Round(NewPrice / Item.Price, 0) : 0; } } 
    }
}
