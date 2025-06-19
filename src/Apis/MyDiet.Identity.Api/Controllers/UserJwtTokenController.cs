using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;

namespace MyDiet.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserJwtTokenController : AGenericController
    {
        private readonly IJwtTokenManager<UserClaimDto, RsaSecurityKey, JsonWebKeySetDto> _jwtTokenManager;

        public UserJwtTokenController(IJwtTokenManager<UserClaimDto, RsaSecurityKey, JsonWebKeySetDto> jwtTokenManager)
        {
            _jwtTokenManager = jwtTokenManager;
        }

        [HttpPost]
        public async Task<IActionResult> GetTokenAsync(UserClaimDto claimDto)
        {

            var res = await _jwtTokenManager.GenerateTokenAsync(claimDto);
            return ComposeResult(res);
        }
    }
}