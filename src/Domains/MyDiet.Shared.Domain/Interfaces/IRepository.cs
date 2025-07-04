using System.Linq.Expressions;

namespace MyDiet.Shared.Domain.Interfaces
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
        where TKey : notnull
    {
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> AddAsync(TEntity? entity);
        Task<TEntity?> UpdateAsync(TEntity? entity);
        Task<TEntity?> DeleteAsync(TKey id);

        //TODO: Add filters
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filters = null);
    }
}
