using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IJwtKeyService<TCreateData, TGetData> where TCreateData : class where TGetData : class
    {
        Task<ApiResponse<TCreateData>> CreatePrivateKeyAsync();
        Task<ApiResponse<TGetData>> GetPublicKeyAsync();
    }
}
