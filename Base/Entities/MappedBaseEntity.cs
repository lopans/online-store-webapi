using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Entities
{
    [ComplexType]
    public class MappedBaseEntity
    {
        public int ID { get; set; }
        public string TypeName { get; set; }
    }
}
