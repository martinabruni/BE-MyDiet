using BaseUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Auth.Domain.Managers;

namespace MyDiet.Session.Api.Controllers
{
    [Authorize(Policy = "Admin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class KeyPairController : ControllerBase
    {
        private readonly IKeyPairManager _keyPairManager;

        public KeyPairController(IKeyPairManager keyPairManager)
        {
            _keyPairManager = keyPairManager;
        }

        [HttpGet]
        public async Task<IActionResult> RegenerateAsync()
        {
            var response = await _keyPairManager.RigenerateAsync();
            return response.ToActionResult();
        }
    }
}
