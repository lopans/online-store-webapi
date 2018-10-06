using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Base.DAL
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public bool Hidden { get; set; }
        [DataMember]
        public double SortOrder { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
