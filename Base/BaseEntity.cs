using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Base.DAL
{
    [Serializable]
    //[DataContract]
    [JsonObject]

    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        //[DataMember]
        public int ID { get; set; }
        //[DataMember]
        public bool Hidden { get; set; }
        //[DataMember]
        public double SortOrder { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
