using Microsoft.EntityFrameworkCore;
using MyDiet.Core.Domain.Interfaces;
using MyDiet.Core.Sql.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Sql.Abstractions
{
    public class AGenericRepository<TKey, TDatabase> : IRepository<TKey, TDatabase> where TDatabase : class, IEntity<TKey>, IAuditable
    {
        private readonly MyDietCoreDbContext _dbContext;
        private readonly DbSet<TDatabase> _dbSet;

        public AGenericRepository(MyDietCoreDbContext dbContext, DbSet<TDatabase> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TDatabase>();
        }

        public async Task<TDatabase> AddAsync(TDatabase entity, CancellationToken ct)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity, ct);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<TDatabase?> DeleteAsync(TKey id, CancellationToken ct)
        {
            var entity = await _dbSet.FindAsync(id, ct);

            if (entity is null)
                return null;

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<IEnumerable<TDatabase>> GetAsync(CancellationToken ct, params Expression<Func<TDatabase, bool>>[]? filters)
        {
            if (filters is null)
                return await _dbSet.ToListAsync(ct);

            IQueryable<TDatabase> query = _dbSet;

            foreach (var filter in filters)
                query = query.Where(filter);

            return await query.ToListAsync(ct);
        }

        public async Task<TDatabase?> GetByIdAsync(TKey id, CancellationToken ct)
        {
            var entity = await _dbSet.FindAsync(id, ct);

            if (entity is null)
                return null;

            return entity;
        }

        public async Task<TDatabase?> UpdateAsync(TDatabase entity, CancellationToken ct)
        {
            var existingEntity = await _dbSet.FindAsync(entity.Id, ct);

            if (existingEntity is null)
                return null;

            //TODO: assegnare la stessa creationDate di existingEntity?

            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync(ct);
            return entity;
        }
    }
}
