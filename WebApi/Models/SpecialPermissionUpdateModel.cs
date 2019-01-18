using Base.Enums;

namespace WebApi.Models
{
    public class SpecialPermissionUpdateModel
    {
        public string RoleID { get; set; }
        public int PermissionID { get; set; }
        public bool IsEnabled { get; set; }
    }
}