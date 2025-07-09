using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Responses;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Validation
{
    internal class DietExistenceValidationHandler : BaseValidationHandler<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>
    {
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly DietMessage _dietMessage;

        public DietExistenceValidationHandler(IService<DietDto, Diet, int> dietService, DietMessage dietMessage)
        {
            _dietService = dietService;
            _dietMessage = dietMessage;
        }

        protected override async Task<BusinessResponse<DietDto>> ValidateAsync(CreateDietRequest request, ValidationContext<CoreValidationContext<DietDto, int>> validation)
        {
            var userId = validation.Context.UserId;

            var existingDietRes = await _dietService.FindAsync(d => d.Name == request.Name && d.UserId == userId);
            
            if (existingDietRes.Data is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_dietMessage.ErrorRetrievingEntities);
            }
            if (existingDietRes.Data.ToList().Count != 0)
            {
                return BusinessResponse<DietDto>.BadRequest(_dietMessage.DietAlreadyExists);
            }
            
            return BusinessResponse<DietDto>.Ok($"{nameof(DietExistenceValidationHandler)} passed");
        }
    }
}
