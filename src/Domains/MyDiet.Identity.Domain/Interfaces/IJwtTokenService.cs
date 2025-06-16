using MyDiet.Shared.Domain.Responses;

namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IJwtTokenService<TClaim> where TClaim : class
    {
        Task<ApiResponse<string>> GenerateTokenAsync(TClaim claimDto);
    }
}
