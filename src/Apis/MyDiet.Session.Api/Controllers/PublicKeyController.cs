using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Session.Domain.Models;
using MyDiet.Session.Domain.Responses;
using MyDiet.Session.Domain.Services;

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
            var response = await _publicKeyService.ExistsAsync();
            return response.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync()
        {
            var response = await _publicKeyService.CreateAsync();
            return response.ToActionResult();
        }
    }
}
