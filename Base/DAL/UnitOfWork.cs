using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Base.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Dispose()
        {
            if(_dbContext != null)
            {
                _dbContext.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            return new Repository<TEntity>(_dbContext);
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
