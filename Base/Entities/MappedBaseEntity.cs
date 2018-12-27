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
        public string TypeName { get; set; }
    }
}
