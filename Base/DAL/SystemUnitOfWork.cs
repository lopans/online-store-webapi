using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Base.DAL
{
    public class SystemUnitOfWork : UnitOfWork, ISystemUnitOfWork
    {
        public SystemUnitOfWork(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
