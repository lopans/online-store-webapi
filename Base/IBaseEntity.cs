using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DAL
{
    public interface IBaseEntity
    {
        int ID { get; set; }
        bool Hidden { get; set; }
        double SortOrder { get; set; }
        byte[] RowVersion { get; set; }
    }
}
