using MyDiet.Session.Domain.Mappers;
using MyDiet.Session.Domain.Models;
using MyDiet.Session.Domain.Options;

namespace MyDiet.Session.Business.Mappers
{
    internal class OpenIdMapper : IMapper<OpenIdOption, OpenId>
    {
        public OpenId Map(OpenIdOption input)
        {
            return new OpenId()
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
