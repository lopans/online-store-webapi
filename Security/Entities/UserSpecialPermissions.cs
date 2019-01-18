using Base.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Core
{
    public class RoleSpecialPermissions: BaseEntity
    {
        public int SpecialPermissionID { get; set; }
        public SpecialPermission SpecialPermission{ get; set; }
        public string RoleID { get; set; }
        [ForeignKey("RoleID"), InverseProperty("SpecialPermissions")]
        public Role Role { get; set; }
    }
}
