using BaseUtility;
using MyDiet.Auth.Domain.Dtos.Responses;
using System.IdentityModel.Tokens.Jwt;

namespace MyDiet.Auth.Business.Mappers
{
    public class TokenMapper : IMapper<JwtSecurityToken, TokenResponse>
    {
        public TokenResponse Map(JwtSecurityToken input)
        {
            var handler = new JwtSecurityTokenHandler();
            string tokenString = handler.WriteToken(input);

            return new TokenResponse
            {
                Token = tokenString,
                TokenExpiration = input.ValidTo
            };
        }
    }
}
