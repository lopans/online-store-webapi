using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO.Core
{
    public class RoleSpecialPermissionDTO
    {
        public int PermissionID { get; set; }
        public string PermissionTitle { get; set; }
        public bool IsEnabled { get; set; }
    }
}
