using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Validators.PlanValidators
{
    internal class PlanCreateParentValidator : BaseValidationHandler<CreatePlanRequest, PlanDto, CoreValidationContext<PlanDto, int>>
    {
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly ResponseMessage _message;

        public PlanCreateParentValidator(IService<DietDto, Diet, int> dietService, ResponseMessage message)
        {
            _dietService = dietService;
            _message = message;
        }

        protected override async Task<BusinessResponse<PlanDto>> ValidateAsync(CreatePlanRequest request, ContextProvider<CoreValidationContext<PlanDto, int>> context)
        {
            if (context.Context.Data is null)
            {
                return BusinessResponse<PlanDto>.InternalServerError(_message.ErrorRetrievingEntity);
            }
            var dietRes = await _dietService.GetByIdAsync(context.Context.Data.DietId);
            if (dietRes.Data is null)
            {
                return BusinessResponse<PlanDto>.NotFound(_message.EntityNotFound);
            }

            if (dietRes.Data.Id != context.Context.Data.DietId)
            {
                return BusinessResponse<PlanDto>.Unauthorize(_message.InvalidRequest);
            }

            if (dietRes.Data.UserId != context.Context.UserId)
            {
                return BusinessResponse<PlanDto>.Unauthorize(_message.InvalidRequest);
            }

            return BusinessResponse<PlanDto>.Ok(context.Context.Data, $"{this.GetType().Name}");
        }
    }
}
