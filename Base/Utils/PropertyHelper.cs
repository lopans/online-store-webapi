using Base.DAL;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.Utils
{
    public static class PropertyHelper
    {
        public static void SetPropertyValue<T, TProperty>(this T target, Expression<Func<T, TProperty>> memberLamda, TProperty value)
        {
            MemberExpression memberExpression = null;

            if (memberLamda.Body.NodeType == ExpressionType.Convert)
            {
                UnaryExpression body = (UnaryExpression)memberLamda.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (memberLamda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = memberLamda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                PropertyInfo property = memberExpression.Member as PropertyInfo;

                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }

        public static T ChangeProperty<T, TProperty>(this T entity, IUnitOfWork uofw, 
            Expression<Func<T, TProperty>> propFunc,
            TProperty value) where T: BaseEntity
        {
            if (entity.ID == 0)
                throw new ArgumentException("ID must be greater than zero", "entity");
            return uofw.GetRepository<T>().ChangeProperty(entity.ID, propFunc, value, entity.RowVersion, entity);
        }
    }
}
