using Microsoft.AspNetCore.Mvc;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;
using System.Security.Cryptography;

namespace MyDiet.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserJwtTokenController : GenericController
    {
        private readonly IJwtTokenService<UserClaimDto, RSA> _jwtTokenService;

        public UserJwtTokenController(IJwtTokenService<UserClaimDto, RSA> jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPemPublicKey()
        {
            //TOOD: return pem format
            var publicKeyRes = await _jwtTokenService.GetPemPublicKey();
            return ComposeResult(publicKeyRes);
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicKey()
        {
            var publicKeyRes = await _jwtTokenService.GetPublicKeyAsync();
            return ComposeResult(publicKeyRes);
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
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