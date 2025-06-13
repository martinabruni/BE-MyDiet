using Microsoft.AspNetCore.Mvc;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;

namespace MyDiet.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserJwtTokenController : GenericController
    {
        private readonly IJwtTokenService<UserClaimDto> _jwtTokenService;

        public UserJwtTokenController(IJwtTokenService<UserClaimDto> jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicKeyAsync()
        {
            var publicKeyRes = await _jwtTokenService.GetPublicKeyAsync();
            return ComposeResult(publicKeyRes);
        }

        [HttpGet]
        public async Task<IActionResult> GetTokenAsync()
        {
            var userClaim = new UserClaimDto
            {
                // TODO: replace with real values
                UserId = Guid.NewGuid()
            };
            var tokenRes = await _jwtTokenService.GenerateTokenAsync(userClaim);
            return ComposeResult(tokenRes);
        }
    }
}