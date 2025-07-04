using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Identity.Domain.Options;
using MyDiet.Shared.Domain.Mappers;

namespace MyDiet.Identity.Business.Mappers
{
    internal class OpenIdMapper : IMapper<OpenIdOption, OpenIdConfigurationDto>
    {
        public OpenIdConfigurationDto Map(OpenIdOption input)
        {
            return new OpenIdConfigurationDto()
            {
                Issuer = input.Issuer,
                AuthorizationEndpoint = input.AuthorizationEndpoint,
                TokenEndpoint = input.TokenEndpoint,
                JwksUri = input.JwksUri,
                IdTokenSigningAlgorithms = input.IdTokenSigningAlgorithms,
                ClaimsSupported = input.ClaimsSupported,
            };
        }
    }
}
