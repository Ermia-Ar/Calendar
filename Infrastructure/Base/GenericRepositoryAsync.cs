using Core.Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Base
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {

        protected readonly ApplicationContext _dbContext;

        public GenericRepositoryAsync(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(string id, CancellationToken token)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new Exception("id is invalid");
            }
            return entity;
        }

        public IQueryable<T> GetTableNoTracking(CancellationToken token)
        {
            return _dbContext.Set<T>().AsNoTracking().AsQueryable();
        }

        public virtual async Task AddRangeAsync(ICollection<T> entities, CancellationToken token)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken token)
        {
            await _dbContext.Set<T>().AddAsync(entity);

            return entity;
        }

        public virtual void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public virtual async Task DeleteAsyncById(string id, CancellationToken token)
        {
            var entity = await GetByIdAsync(id , token);

            _dbContext.Set<T>().Remove(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public virtual void DeleteRange(ICollection<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollBack()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

        public IQueryable<T> GetTableAsTracking(CancellationToken token)
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public virtual void UpdateRange(ICollection<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
        }
    }

}
