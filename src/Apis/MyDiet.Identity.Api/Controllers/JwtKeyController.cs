using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;

namespace MyDiet.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class JwtKeyController : AGenericController
    {
        private readonly IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto> _jwtKeyService;

        public JwtKeyController(ILogger<JwtKeyController> logger, IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto> jwtKeyService)
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


        [HttpGet("~/.well-known/openid-configuration")]
        public async Task<IActionResult> GetOpenIdConfigurationAsync()
        {
            var openIdConfigurationRes = await _jwtKeyService.GetOpenIdConfigurationKeyAsync();
            return ComposeResult(openIdConfigurationRes);
        }
    }
}
