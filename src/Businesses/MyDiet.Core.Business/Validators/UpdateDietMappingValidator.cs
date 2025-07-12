using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validators
{
    internal class UpdateDietMappingValidator : BaseValidationHandler<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>
    {
        private readonly IMapper<CreateDietRequest, DietDto> _createRequestToDietDtoMapper;
        private readonly ResponseMessage _message;

        public UpdateDietMappingValidator(IMapper<CreateDietRequest, DietDto> createRequestToDietDtoMapper, ResponseMessage message)
        {
            _createRequestToDietDtoMapper = createRequestToDietDtoMapper;
            _message = message;
        }

        protected override Task<BusinessResponse<DietDto>> ValidateAsync(CreateDietRequest request, ValidationContext<CoreValidationContext<DietDto, int>> context)
        {
            var oldDto = context.Context.Data;

            if (oldDto is null)
            {
                return Task.FromResult(BusinessResponse<DietDto>.InternalServerError(_message.ErrorRetrievingEntity));
            }

            var newDto = _createRequestToDietDtoMapper.Map(request);
            if (newDto is null)
            {
                return Task.FromResult(BusinessResponse<DietDto>.InternalServerError(_message.ErrorMapping));
            }

            newDto.CreatedAt = oldDto.CreatedAt;
            newDto.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult(BusinessResponse<DietDto>.Ok(newDto, $"{nameof(CreateDietMappingValidator)} passed"));
        }
    }
}
