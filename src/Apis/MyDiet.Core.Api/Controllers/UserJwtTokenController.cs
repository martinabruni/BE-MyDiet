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
            return Ok(new { PublicKey = publicKey });
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            var userClaim = new UserClaimDto
            {
                // TODO: replace with real values
                UserId = Guid.NewGuid() 
            };
            var token = await _jwtTokenService.GenerateTokenAsync(userClaim);
            return Ok(new { Token = token });
        }
    }
}