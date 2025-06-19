using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Shared.Domain.Interfaces
{
    public interface IService<TDomain, TKey> where TDomain : class
    {
        Task<ApiResponse<TDomain>> GetAsync(TKey key);
    }
}
