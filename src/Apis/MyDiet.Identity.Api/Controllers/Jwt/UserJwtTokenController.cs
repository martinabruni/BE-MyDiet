using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Identity.Domain.Managers;

namespace MyDiet.Identity.Api.Controllers.Jwt
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserJwtTokenController : ControllerBase
    {
        private readonly IJwtTokenManager<UserRegistrationDto, UserClaims, RsaSecurityKey, JsonWebKeySetDto> _jwtTokenManager;
        public UserJwtTokenController(IJwtTokenManager<UserRegistrationDto, UserClaims, RsaSecurityKey, JsonWebKeySetDto> jwtTokenManager)
        {
            _jwtTokenManager = jwtTokenManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTokenAsync([FromBody] UserRegistrationDto userRegistrationDto)
        {
            var res = await _jwtTokenManager.GenerateTokenAsync(userRegistrationDto);
            return this.ComposeResult(res);
        }
    }
}
