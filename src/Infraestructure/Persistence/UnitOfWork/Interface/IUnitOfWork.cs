using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Repository.Interface;

namespace Persistence.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        // Generic repository access
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        // Unit of Work operations
        void SaveChanges();
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
