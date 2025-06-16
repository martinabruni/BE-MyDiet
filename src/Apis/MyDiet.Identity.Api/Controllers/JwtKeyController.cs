using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;

namespace MyDiet.Identity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtKeyController : GenericController
    {
        private readonly IJwtKeyService<RsaSecurityKey, JwkSetDto> _jwtKeyService;

        public JwtKeyController(IJwtKeyService<RsaSecurityKey, JwkSetDto> jwtKeyService)
        {
            _jwtKeyService = jwtKeyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrivateKeyAsync()
        {
            var keyRes = await _jwtKeyService.CreatePrivateKeyAsync();
            return ComposeResult(keyRes);
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicKeyAsync()
        {
            var keyRes = await _jwtKeyService.GetPublicKeyAsync();
            return ComposeResult(keyRes);
        }

    }
}
