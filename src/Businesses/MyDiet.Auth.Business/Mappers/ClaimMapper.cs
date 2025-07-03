using BaseUtility;
using MyDiet.Auth.Domain.Dtos.Claims;
using System.Security.Claims;

namespace MyDiet.Auth.Business.Mappers
{
    internal class ClaimMapper : IMapper<UserClaims, List<Claim>>
    {
        public List<Claim> Map(UserClaims input)
        {
            return [
                new Claim("userId", input.UserId.ToString()),
                new Claim("username", input.Username ?? string.Empty),
                new Claim(ClaimTypes.Role, input.Role.ToString())
            ];
        }
    }
}
