using BaseUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Auth.Domain.Managers;

namespace MyDiet.Shared.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class KeyPairController : ControllerBase
    {
        private readonly IKeyPairManager _keyPairManager;

        public KeyPairController(IKeyPairManager keyPairManager)
        {
            _keyPairManager = keyPairManager;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        private async Task<IActionResult> RegenerateAsync()
        {
            var response = await _keyPairManager.RegenerateAsync();
            return response.ToActionResult();
        }
    }
}
