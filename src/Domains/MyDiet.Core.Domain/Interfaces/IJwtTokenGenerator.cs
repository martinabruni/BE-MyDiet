namespace MyDiet.Core.Domain.Interfaces
{
    public interface IJwtTokenGenerator<TClaim>
    {
        //TODO: Add constrainst
        Task<string> GenerateTokenAsync(TClaim claimDto);
    }
}
