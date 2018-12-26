using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.DAL
{
    [Serializable]
    [JsonObject]

    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        [Column(Order = 0)]
        public int ID { get; set; }
        public bool Hidden { get; set; }
        public double SortOrder { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }


       
    }
    
}
