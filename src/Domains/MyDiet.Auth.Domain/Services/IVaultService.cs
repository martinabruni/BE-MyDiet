using BaseUtility;

namespace MyDiet.Auth.Domain.Services
{
    public interface IVaultService<TData>
        where TData : class
    {
        Task<BusinessResponse<TData>> GetDeletedAsync();
        Task<BusinessResponse<TData>> CreateAsync();
        Task<BusinessResponse<TData>> PurgeDeletedAsync();
        Task<BusinessResponse<TData>> GetAsync();
    }
}
