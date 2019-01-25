using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ListViewData<T>
    {
        public ListViewData(IEnumerable<T> data)
        {
            Data = data;
        }
        public IEnumerable<T> Data { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}