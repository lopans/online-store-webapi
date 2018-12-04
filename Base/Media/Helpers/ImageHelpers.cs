using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Base.Media.Helpers
{
    public static class ImageHelpers
    {
        public static Guid? GetGuidOrNull<T>(this T entity, Expression<Func<T, FileData>> expression)
            where T: BaseEntity
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
                PropertyInfo property = memberExpression.Member as PropertyInfo;
                var data = property.GetValue(entity) as FileData;
                return data == null ? null : (Guid?)data.FileID;
            }
            else
                throw new NotSupportedException();
        }

        public static Guid? GetGuidOrNull(this FileData data)
        {
            return data != null ? (Guid?)data.FileID : null;
        }
    }
}
