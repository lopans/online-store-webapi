using System;
using System.Threading.Tasks;

namespace Base.DAL
{
    public interface IUnitOfWork: IDisposable
    {
        Task<int> SaveChangesAsync();
        int SaveChanges();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    }

    public interface ISystemUnitOfWork : IUnitOfWork
    {

    }
}
