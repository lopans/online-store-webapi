using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Entities
{
    public class Role: IdentityRole
    {
        public Role(): base()
        {
        }

        public Role(string name): base(name)
        {

        }
        [InverseProperty("Role")]
        public ICollection<RoleSpecialPermissions> SpecialPermissions { get; set; }
    }
}
