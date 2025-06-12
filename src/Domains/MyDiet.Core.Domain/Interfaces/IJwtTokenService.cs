namespace MyDiet.Core.Domain.Interfaces
{
    public interface IJwtTokenService<TClaim>
    {
        Task<string> GenerateTokenAsync(TClaim claimDto);
    }
}
