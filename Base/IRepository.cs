using Base.DAL;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Base
{
    public interface IRepository<T> where T: BaseEntity
    {
        IQueryable<T> All();

        T Create(T t);
        int Delete(T t);
        int Delete(Expression<Func<T, bool>> predicate);
        int Update(T t);

        void Attach(T t);
        void Detach(T t);
        void ChangeProperty<TProperty>(int id, 
            Expression<Func<T, TProperty>> propFunc, 
            TProperty value, 
            byte[] rowVersion = null);
    }
}
