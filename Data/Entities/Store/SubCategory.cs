﻿using Base;
using Base.DAL;
using System.Collections.Generic;

namespace Data.Entities.Store
{
    public class SubCategory: BaseEntity, IClientEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual FileData Image { get; set; }
        public int? ImageID { get; set; }
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
