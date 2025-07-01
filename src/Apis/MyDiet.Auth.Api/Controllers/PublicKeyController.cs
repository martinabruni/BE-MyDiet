using BaseUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Auth.Domain.Models;
using MyDiet.Auth.Domain.Services;

namespace MyDiet.Session.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PublicKeyController : ControllerBase
    {
        private readonly IVaultService<JsonWebKeySetDto> _publicKeyService;

        public PublicKeyController(IVaultService<JsonWebKeySetDto> publicKeyService)
        {
            _publicKeyService = publicKeyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _publicKeyService.GetAsync();
            return response.ToActionResult();
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync()
        {
            var response = await _publicKeyService.CreateAsync();
            return response.ToActionResult();
        }
    }
}
