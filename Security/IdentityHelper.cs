using System;
using System.Linq;
using System.Security.Claims;

namespace Security
{
    public static class IdentityHelper
    {
        public static Guid? GetUserID(this ClaimsPrincipal identity)
        {
            var val = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (val == null)
                return null;
            if (Guid.TryParse(val, out var result))
                return result;
            return null;
        }
    }
}
