using Base.DAL;
using Base.Entities;
using Base.Enums;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Entities
{
    public class AccessLevel: BaseEntity
    {
        public string RoleID { get; set; }
        [ForeignKey("RoleID")]
        public IdentityRole Role { get; set; }
        public MappedBaseEntity Entity { get; set; }
        public AccessModifier AccessModifier { get; set; }
    }
}
