using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;

namespace Core.Domain.Interfaces.Repositories
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        void DeleteRange(ICollection<T> entities);
        Task<T> GetByIdAsync(string id, CancellationToken token);
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task Commit();
        Task RollBack();
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        Task<T> AddAsync(T entity, CancellationToken token);
        Task AddRangeAsync(ICollection<T> entities, CancellationToken token);
        void Update(T entity);
        void UpdateRange(ICollection<T> entities);
        void Delete(T entity);
        Task DeleteAsyncById(string id, CancellationToken token);

    }
}
