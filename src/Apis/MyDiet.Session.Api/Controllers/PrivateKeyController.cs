using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Session.Domain.Services;

namespace MyDiet.Session.Api.Controllers
{
    [Authorize(Policy = "Admin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PrivateKeyController : ControllerBase
    {
        private readonly IVaultService<KeyVaultSecret> _privateKeyService;

        public PrivateKeyController(IVaultService<KeyVaultSecret> privateKeyService)
        {
            _privateKeyService = privateKeyService;
        }

        [HttpGet]
        public async Task<IActionResult> ExistsAsync()
        {
            var response = await _privateKeyService.ExistsAsync();
            return response.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync()
        {
            var response = await _privateKeyService.CreateAsync();
            return response.ToActionResult();
        }
    }
}
