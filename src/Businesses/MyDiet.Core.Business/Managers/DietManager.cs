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
        private ValidationContext<CoreValidationContext<DietDto, int>> _validation = new() { Context = new() };
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly DietValidationPipeline _validationPipeline;

        public DietManager(IService<DietDto, Diet, int> dietService, DietValidationPipeline validationPipeline)
        {
            _dietService = dietService;
            _validationPipeline = validationPipeline;
        }

        public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
        {
            _validation.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _validationPipeline.CommonValidators.ValidateAsync(request, _validation);
            var dietDto = validationRes.Data;

            if (dietDto is null)
            {
                return validationRes;
            }

            return await _dietService.CreateAsync(dietDto);
        }

        public async Task<BusinessResponse<DietDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            _validation.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _validationPipeline.DeleteValidators.ValidateAsync(id, _validation);

            return await _dietService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<DietDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            _validation.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _validationPipeline.GetByIdValidators.ValidateAsync(id, _validation);
            
            return await _dietService.GetByIdAsync(id);
        }

        public async Task<BusinessResponse<IEnumerable<DietDto>>> GetByUserIdAsync(Claim? userIdClaim)
        {
            _validation.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _validationPipeline.GetByUserIdValidators.ValidateAsync(userIdClaim, _validation);

            return await _dietService.FindAsync(d => d.UserId == validationRes.Data.UserId);
        }

        public async Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim)
        {
            _validation.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _validationPipeline.UpdateValidators.ValidateAsync(request, _validation);

            if (validationRes.Data is null)
            {
                return validationRes;
            }

            return await _dietService.UpdateAsync(validationRes.Data);
        }
    }
}
