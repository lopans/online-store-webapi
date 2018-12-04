using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Base.Enums.Extensions
{
    public static class EnumHelpers
    {
        public static string GetTitle(this Enum value)
        {
            var attr = GetUiEnumValue(value);

            return attr?.Title ?? value.ToString();
        }

        public static string GetColor(this Enum value)
        {
            var attr = GetUiEnumValue(value);

            return attr?.Color ?? value.ToString();
        }

        public static string GetIcon(this Enum value)
        {
            var attr = GetUiEnumValue(value);

            return attr?.Icon ?? value.ToString();
        }
        public static int GetValue(this Enum value)
        {
            return (int)((object)value);
        }
        private static UiEnumValueAttribute GetUiEnumValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            return fieldInfo.GetCustomAttribute<UiEnumValueAttribute>();
        }
    }
}
