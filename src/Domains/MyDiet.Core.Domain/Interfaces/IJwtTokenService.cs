namespace MyDiet.Core.Domain.Interfaces
{
    public interface IJwtTokenService<TDto>
    {
        Task<string> GenerateTokenAsync(TDto claimDto);
    }
}
