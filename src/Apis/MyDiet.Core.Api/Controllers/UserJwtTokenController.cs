using Microsoft.AspNetCore.Mvc;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserJwtTokenController : ControllerBase
    {
        private readonly IJwtTokenService<UserClaimDto, RSA> _jwtTokenService;

        public UserJwtTokenController(IJwtTokenService<UserClaimDto, RSA> jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicKey()
        {
            var publicKey = await _jwtTokenService.GetPublicKey();
            return Ok(new { PublicKey = Convert.ToBase64String(publicKey) });
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            var userClaim = new UserClaimDto
            {
                UserId = Guid.NewGuid() // This should be replaced with actual user ID retrieval logic
            };
            var token = await _jwtTokenService.GenerateTokenAsync(userClaim);
            return Ok(new { Token = token });
        }
    }
}