using Data.ContextInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ContextInitializer
{
    public interface IContextInitializer
    {
        void Initialize(DataContext context);
    }
    internal class BaseContextInitializer
    {
        private readonly RoleInitializer _roleInitializer;
        private readonly UserInitializer _userInitializer;
        private readonly SpecialPermissionsInitializer _specialPermissionsInitializer;
        private readonly PermissionsInitializer _permissionsInitializer;
        public BaseContextInitializer()
        {
            _roleInitializer = new RoleInitializer();
            _specialPermissionsInitializer = new SpecialPermissionsInitializer();
            _permissionsInitializer = new PermissionsInitializer();
            _userInitializer = new UserInitializer();
        }
        public void Initialize(DataContext context)
        {
            // TODO: drag uofw here from initialize of IContextInitializer
            _roleInitializer.Initialize(context);
            _userInitializer.Initialize(context);
            _permissionsInitializer.Initialize(context);
            _specialPermissionsInitializer.Initialize(context);
        }
    }
}
