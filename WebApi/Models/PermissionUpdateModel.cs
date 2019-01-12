using Base.Enums;

namespace WebApi.Models
{
    public class PermissionUpdateModel
    {
        public string RoleID { get; set; }
        public AccessModifier Permission { get; set; }
        public string EntityType { get; set; }
        public bool IsEnabled { get; set; }
    }
}