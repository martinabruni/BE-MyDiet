namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IJwtTokenGenerator<TClaim> where TClaim : class
    {
        Task<string> GenerateTokenAsync(TClaim claimDto);
    }
}
