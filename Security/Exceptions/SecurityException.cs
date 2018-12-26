using Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Security.Exceptions
{
    public class SecurityException : Exception
    {
        public SecurityException(Type entityType, AccessModifier modifier)
            :base($"Permission {modifier.ToString()} is not accessible with your roles for entity {entityType.Name}")
        {
            
        }
    }
}
