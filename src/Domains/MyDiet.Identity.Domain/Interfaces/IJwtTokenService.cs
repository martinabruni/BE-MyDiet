using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IJwtTokenService<TClaim> where TClaim : class
    {
        Task<ApiDataResponse<string>> GetPublicKeyAsync();
        Task<ApiDataResponse<string>> GenerateTokenAsync(TClaim claimDto);
    }
}
