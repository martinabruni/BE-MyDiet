using BaseUtility;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Managers;

namespace MyDiet.Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenManager _tokenManager;

        public TokenController(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTokenAsync([FromBody]UserClaims claims)
        {
            var response = await _tokenManager.GenerateTokenAsync(claims);
            return response.ToActionResult();
        }
    }
}
