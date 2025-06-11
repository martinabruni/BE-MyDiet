namespace MyDiet.Core.Domain.Interfaces
{
    public interface IJwtTokenGenerator<TDto>
    {
        //TODO: Add constrainst
        Task<string> GenerateTokenAsync(TDto claimDto);
    }
}
