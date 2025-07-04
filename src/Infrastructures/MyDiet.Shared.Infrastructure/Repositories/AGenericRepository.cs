using Microsoft.EntityFrameworkCore;
using MyDiet.Shared.Domain.Interfaces;
using System.Linq.Expressions;

namespace MyDiet.Shared.Infrastructure.Repositories
{
    internal class AGenericRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : notnull
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly IDatabase _db;

        public AGenericRepository(IDatabase db)
        {
            _db = db;
            _dbSet = db.Context.Set<TEntity>();
        }

        public async Task<TEntity?> AddAsync(TEntity? entity)
        {
            if (entity == null)
            {
                return null;
            }
            await _dbSet.AddAsync(entity);
            await _db.Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity?> DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }
            _dbSet.Remove(entity);
            await _db.Context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filters = null)
        {
            if (filters is null)
            {
                return await GetAllAsync();
            }
            //TODO: see if query is the correct approach here
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            query = query.Where(filters);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity?> UpdateAsync(TEntity? entity)
        {
            if (entity == null)
            {
                return null;
            }
            _dbSet.Update(entity);
            await _db.Context.SaveChangesAsync();
            return entity;
        }
    }
}
