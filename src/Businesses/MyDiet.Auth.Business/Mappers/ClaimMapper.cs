using BaseUtility;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using System.Security.Claims;

namespace MyDiet.Auth.Business.Mappers
{
    internal class ClaimMapper : IMapper<UserClaims, List<Claim>>
    {
        public List<Claim> Map(UserClaims input)
        {
            return [
                new Claim("UserId", input.UserId.ToString()),
                new Claim("Role", input.Role.ToString())
            ];
        }
    }
}
