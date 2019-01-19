using Base.DAL;
using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Entities
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
