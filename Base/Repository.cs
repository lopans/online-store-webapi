using Base.DAL;
using Base.Utils;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private DbSet<T> DbSet => _context.Set<T>();

        public Repository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<T> All()
        {
            return DbSet.AsQueryable<T>();
        }

        public void Attach(T t)
        {
            if(!DbSet.Local.Any(x => x.ID == t.ID))
                DbSet.Attach(t);
        }

        public T ChangeProperty<TProperty>(int id, Expression<Func<T, TProperty>> propFunc,
            TProperty value, byte[] rowVersion = null, T previousState = null)
        {
            var obj = previousState ?? Activator.CreateInstance<T>();
            if (previousState == null)
            {
                obj.ID = id;
                obj.RowVersion = rowVersion ?? All().Where(x => x.ID == id).Select(x => x.RowVersion).Single();
            }

            Attach(obj);
            obj.SetPropertyValue(propFunc, value);

            _context.Entry(obj).Property(propFunc).IsModified = true;
            return obj;
        }

        public void SetFromObject<TObject>(int id, TObject instance)
        {
            var obj = All().Where(x => x.ID == id).SingleOrDefault();
            if (obj == null)
                throw new Exception("Object not found or it is dublicate");
            Attach(obj);
            _context.Entry(obj).CurrentValues.SetValues(instance);
        }

        public T Create(T t)
        {
            var ret = DbSet.Add(t);
            return ret;
        }

        public int Delete(T t)
        {
            DbSet.Remove(t);
            return 0;
        }

        public virtual int Update(T t)
        {
            if (_context.Entry(t).State != EntityState.Detached) return 0;

            DbSet.Attach(t);
            _context.Entry(t).State = EntityState.Modified;

            return 0;
        }

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            var toDel = All().Where(predicate).ToArray();
            foreach (var item in toDel)
            {
                DbSet.Remove(item);
            }
            return toDel.Length;
        }

        public void Detach(T t)
        {
            _context.Entry(t).State = EntityState.Detached;
        }
    }
}
