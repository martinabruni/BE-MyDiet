using MyDiet.Core.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Core.Security.Abstractions
{
    internal abstract class AGenericJwtTokenGenerator<TDto> : IJwtTokenGenerator<TDto>
    {
        public readonly JwtSettings _jwtSettings;
        public readonly IKeyProvider<RSA> _keyProvider;

        protected AGenericJwtTokenGenerator(IKeyProvider<RSA> keyProvider, JwtSettings jwtSettings)
        {
            this._keyProvider = keyProvider;
            this._jwtSettings = jwtSettings;
        }

        public abstract Task<string> GenerateTokenAsync(TDto claimDto);
    }
}
