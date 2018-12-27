using Base.DAL;
using Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services
{
    public interface IAccessService
    {
        void ThrowIfAccessDenied(IUnitOfWork uofw, AccessModifier permission, Type entityType);
        void ThrowIfNotInRole(string role);
    }
}
