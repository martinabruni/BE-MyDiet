using BaseUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DietController : ControllerBase
    {
        private readonly IService<DietDto, Diet, int> _dietService;

        public DietController(IService<DietDto, Diet, int> dietService)
        {
            _dietService = dietService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDietAsync([FromBody] DietDto dietDto)
        {
            var response = await _dietService.CreateAsync(dietDto);
            return response.ToActionResult();
        }
    }
}
