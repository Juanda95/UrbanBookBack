using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Contexts;
using Persistence.Repository;
using Persistence.Repository.Interface;
using Persistence.UnitOfWork.Interface;

namespace Persistence.UnitOfWork
{
    public class ConfUnitOfWork(UrbanBookDbContext dbContext) : IUnitOfWork
    {
   
        private readonly UrbanBookDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        // Implementación del IDisposable para liberar recursos si es necesario
        private bool disposed = false;

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            Type entityType = typeof(TEntity);
            if (!_repositories.ContainsKey(entityType))
            {
                _repositories[entityType] = new Repository<TEntity>(_dbContext);
            }
            return (IRepository<TEntity>)_repositories[entityType];
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync() { return await _dbContext.Database.BeginTransactionAsync(); }

    }
}
