using MyDiet.Core.Domain.Interfaces;

namespace MyDiet.Core.Business.Services
{
    internal abstract class AGenericJwtTokenService<TClaim> : IJwtTokenService<TClaim>
    {
        private readonly IJwtTokenGenerator<TClaim> _jwtTokenGenerator;

        public AGenericJwtTokenService(IJwtTokenGenerator<TClaim> jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> GenerateTokenAsync(TClaim claimDto)
        {
            return await _jwtTokenGenerator.GenerateTokenAsync(claimDto);
        }
    }
}
