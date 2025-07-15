using BaseUtility;
using MyDiet.Core.Business.ValidationPipelines;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Managers;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.Managers
{
    internal class DietManager : BaseManager<CreateDietRequest>, IManager<DietDto, CreateDietRequest, int>
    {
        private ContextProvider<CoreValidationContext<DietDto, int>> _contextProvider = new() { Context = new() };
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly DietValidationPipelineSet _pipelineSet;

        public DietManager(IService<DietDto, Diet, int> dietService, DietValidationPipelineSet pipelineSet)
        {
            _dietService = dietService;
            _pipelineSet = pipelineSet;
        }

        public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
        {
            _contextProvider.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _pipelineSet.CreateValidators.ValidateAsync(request, _contextProvider);

            var dietDto = validationRes.Data;
            if (dietDto is null)
            {
                return validationRes;
            }

            return await _dietService.CreateAsync(dietDto);
        }

        public async Task<BusinessResponse<DietDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            _contextProvider.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _pipelineSet.DeleteValidators.ValidateAsync(id, _contextProvider);

            return await _dietService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<DietDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            _contextProvider.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _pipelineSet.GetByIdValidators.ValidateAsync(id, _contextProvider);

            return validationRes;
        }

        public async Task<BusinessResponse<IEnumerable<DietDto>>> GetByUserIdAsync(Claim? userIdClaim)
        {
            _contextProvider.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _pipelineSet.GetByUserIdValidators.ValidateAsync(userIdClaim, _contextProvider);

            return await _dietService.FindAsync(d => d.UserId == validationRes.Data.UserId);
        }

        //TODO: Non funziona perche l'handler che controlla l'esistenza della dieta e' sbagliato (p.Name == request.Name && p.Id != id)
        public async Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim)
        {
            var dietDto = 
                new DietDto
            {
                Id = id,
                Name = request.Name,
            };

            _contextProvider.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim,
                Data = dietDto
            };

            var validationRes = await _pipelineSet.UpdateValidators.ValidateAsync(dietDto, _contextProvider);

            return await _dietService.UpdateAsync(validationRes.Data);
        }
    }
}
