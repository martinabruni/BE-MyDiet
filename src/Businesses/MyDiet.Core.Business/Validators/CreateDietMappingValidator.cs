using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validators
{
    internal class CreateDietMappingValidator : BaseValidationHandler<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>
    {
        private readonly IMapper<CreateDietRequest, DietDto> _createRequestToDietDtoMapper;
        private readonly ResponseMessage _message;

        public CreateDietMappingValidator(IMapper<CreateDietRequest, DietDto> createRequestToDietDtoMapper, ResponseMessage message)
        {
            _createRequestToDietDtoMapper = createRequestToDietDtoMapper;
            _message = message;
        }

        protected override async Task<BusinessResponse<DietDto>> ValidateAsync(CreateDietRequest request, ValidationContext<CoreValidationContext<DietDto, int>> context)
        {
            var dietDto = _createRequestToDietDtoMapper.Map(request);
            if (dietDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_message.ErrorCreatingEntity);
            }

            dietDto.UserId = context.Context.UserId;
            dietDto.CreatedAt = DateTime.UtcNow;
            dietDto.UpdatedAt = dietDto.CreatedAt;

            return BusinessResponse<DietDto>.Ok(dietDto, $"{nameof(CreateDietMappingValidator)} passed");
        }
    }
}
