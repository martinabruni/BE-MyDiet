using MyDiet.Shared.Domain.Responses;
using System.Linq.Expressions;

namespace MyDiet.Shared.Domain.Interfaces
{
    public interface IService<TDomain, TEntity, TKey>
        where TDomain : class
        where TEntity : class
        where TKey : notnull
    {
        Task<ApiResponse<TDomain>> GetByIdAsync(TKey id);
        Task<ApiResponse<IEnumerable<TDomain>>> GetAllAsync();
        Task<ApiResponse<TDomain>> AddAsync(TDomain? entity);
        Task<ApiResponse<TDomain>> UpdateAsync(TDomain? entity);
        Task<ApiResponse<TDomain>> DeleteAsync(TKey id);

        Task<ApiResponse<IEnumerable<TDomain>>> GetAsync(Expression<Func<TEntity, bool>>? filters = null);
    }
}
