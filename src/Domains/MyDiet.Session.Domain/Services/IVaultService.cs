using MyDiet.Session.Domain.Responses;

namespace MyDiet.Session.Domain.Services
{
    public interface IVaultService<TData>
        where TData : class
    {
        Task<BusinessResponse<TData>> GetDeletedAsync();
        Task<BusinessResponse<TData>> CreateAsync();
        Task<BusinessResponse<TData>> ExistsAsync();
        Task<BusinessResponse<TData>> PurgeDeletedAsync();
    }
}
