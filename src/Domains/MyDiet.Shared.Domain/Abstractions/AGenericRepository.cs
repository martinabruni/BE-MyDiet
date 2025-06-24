
using Microsoft.EntityFrameworkCore;
using MyDiet.Shared.Domain.Interfaces;

namespace MyDiet.Shared.Domain.Abstractions
{
    public class AGenericRepository<TContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : notnull
        where TContext : DbContext
    {
        private readonly IDatabase<TContext> _db;
        private readonly DbSet<TEntity> _dbSet;

        public AGenericRepository(IDatabase<TContext> db)
        {
            _db = db;
            _dbSet = _db.Context.Set<TEntity>();
        }

        public async Task<RepositoryResponse<TEntity>> CreateAsync(TEntity entity)
        {
            if (entity is null)
            {
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.BadRequest,
                    Message = "Entity cannot be null."
                };
            }
            try
            {
                await _dbSet.AddAsync(entity);
                await _db.Context.SaveChangesAsync();
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.Created,
                    Data = entity,
                    Message = "Entity created successfully."
                };
            }
            catch
            {
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = $"Error creating entity"
                };
            }
        }

        public async Task<RepositoryResponse<TEntity>> DeleteAsync(TKey id)
        {
            if (id is null)
            {
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.BadRequest,
                    Message = "ID cannot be null."
                };
            }
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity is null)
                {
                    return new RepositoryResponse<TEntity>
                    {
                        StatusCode = RepositoryCode.NotFound,
                        Message = "Entity not found."
                    };
                }
                _dbSet.Remove(entity);
                await _db.Context.SaveChangesAsync();
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.Ok,
                    Data = entity,
                    Message = "Entity deleted successfully."
                };
            }
            catch
            {
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = "Error deleting entity"
                };
            }
        }

        public async Task<RepositoryResponse<IEnumerable<TEntity>>> FindAsync(Func<TEntity, bool> predicate)
        {
            if (predicate is null)
            {
                return new RepositoryResponse<IEnumerable<TEntity>>
                {
                    StatusCode = RepositoryCode.BadRequest,
                    Message = "Predicate cannot be null."
                };
            }
            try
            {
                var entities = _dbSet.Where(predicate).ToList();
                return new RepositoryResponse<IEnumerable<TEntity>>
                {
                    StatusCode = RepositoryCode.Ok,
                    Data = entities,
                    Message = "Entities found successfully."
                };
            }
            catch
            {
                return new RepositoryResponse<IEnumerable<TEntity>>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = "Error finding entities"
                };
            }
        }

        public async Task<RepositoryResponse<IEnumerable<TEntity>>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet.ToListAsync();
                return new RepositoryResponse<IEnumerable<TEntity>>
                {
                    StatusCode = RepositoryCode.Ok,
                    Data = entities,
                    Message = "Entities retrieved successfully."
                };
            }
            catch
            {
                return new RepositoryResponse<IEnumerable<TEntity>>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = "Error retrieving entities"
                };
            }
        }

        public async Task<RepositoryResponse<TEntity>> GetByIdAsync(TKey id)
        {
            if (id is null)
            {
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.BadRequest,
                    Message = "ID cannot be null."
                };
            }
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity is null)
                {
                    return new RepositoryResponse<TEntity>
                    {
                        StatusCode = RepositoryCode.NotFound,
                        Message = "Entity not found."
                    };
                }
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.Ok,
                    Data = entity,
                    Message = "Entity retrieved successfully."
                };
            }
            catch
            {
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = "Error retrieving entity"
                };
            }
        }

        public async Task<RepositoryResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            if (entity is null)
            {
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.BadRequest,
                    Message = "Entity cannot be null."
                };
            }
            try
            {
                _dbSet.Update(entity);
                await _db.Context.SaveChangesAsync();
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.Ok,
                    Data = entity,
                    Message = "Entity updated successfully."
                };
            }
            catch
            {
                return new RepositoryResponse<TEntity>
                {
                    StatusCode = RepositoryCode.InternalServerError,
                    Message = "Error updating entity"
                };
            }
        }
    }
}
