using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Base
{
    public class AppContext
    {
        public static string UserID
        {
            get
            {
                var identity = HttpContext.Current?.User?.Identity;
                return identity != null ? identity.GetUserId() : null;
            }
        }
    }
}
