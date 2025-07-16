using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietUpdateRequestMapper : BaseValidationHandler<DietDto, DietDto, CoreValidationContext<DietDto, int>>
    {
        private readonly ResponseMessage _message;

        public DietUpdateRequestMapper(ResponseMessage message)
        {
            _message = message;
        }

        protected override Task<BusinessResponse<DietDto>> ValidateAsync(DietDto request, ContextProvider<CoreValidationContext<DietDto, int>> context)
        {
            var newDto = context.Context.Data;
            var oldDto = context.Context.OldData;
            if (newDto is null)
            {
                return Task.FromResult(BusinessResponse<DietDto>.InternalServerError(_message.ErrorRetrievingEntity));
            }
            if (oldDto is null)
            {
                return Task.FromResult(BusinessResponse<DietDto>.InternalServerError(_message.ErrorRetrievingEntity));
            }
            newDto.UserId = oldDto.UserId;
            newDto.CreatedAt = oldDto.CreatedAt;
            newDto.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult(BusinessResponse<DietDto>.Ok(newDto, $"{this.GetType().Name} passed"));
        }
    }
}
