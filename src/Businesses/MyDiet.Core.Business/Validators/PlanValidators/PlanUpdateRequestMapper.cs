using BaseUtility;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validators.PlanValidators
{
    internal class PlanUpdateRequestMapper : BaseValidationHandler<PlanDto, PlanDto, CoreValidationContext<PlanDto, int>>
    {
        private readonly ResponseMessage _message;

        public PlanUpdateRequestMapper(ResponseMessage message)
        {
            _message = message;
        }

        protected override Task<BusinessResponse<PlanDto>> ValidateAsync(PlanDto request, ContextProvider<CoreValidationContext<PlanDto, int>> context)
        {
            var newDto = context.Context.Data;
            var oldDto = context.Context.OldData;
            if (newDto is null)
            {
                return Task.FromResult(BusinessResponse<PlanDto>.InternalServerError(_message.ErrorRetrievingEntity));
            }
            if (oldDto is null)
            {
                return Task.FromResult(BusinessResponse<PlanDto>.InternalServerError(_message.ErrorRetrievingEntity));
            }
            newDto.UserId = oldDto.UserId;
            newDto.CreatedAt = oldDto.CreatedAt;
            newDto.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult(BusinessResponse<PlanDto>.Ok(newDto, $"{this.GetType().Name} passed"));
        }
    }
}
