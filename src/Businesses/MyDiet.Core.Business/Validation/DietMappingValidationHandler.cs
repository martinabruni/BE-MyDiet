using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validation
{
    internal class DietMappingValidationHandler : BaseValidationHandler<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>
    {
        private readonly IMapper<CreateDietRequest, CreateDietDto> _createRequestToCreateDtoMapper;
        private readonly IMapper<CreateDietDto, DietDto> _createDtoToDietDtoMapper;
        private readonly ResponseMessage _message;

        public DietMappingValidationHandler(IMapper<CreateDietRequest, CreateDietDto> createRequestToCreateDtoMapper, ResponseMessage message, IMapper<CreateDietDto, DietDto> createDtoToDietDtoMapper)
        {
            _createRequestToCreateDtoMapper = createRequestToCreateDtoMapper;
            _message = message;
            _createDtoToDietDtoMapper = createDtoToDietDtoMapper;
        }

        protected override async Task<BusinessResponse<DietDto>> ValidateAsync(CreateDietRequest request, ValidationContext<CoreValidationContext<DietDto, int>> context)
        {
            var createDto = _createRequestToCreateDtoMapper.Map(request);
            if (createDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_message.ErrorCreatingEntity);
            }

            createDto.UserId = context.Context.UserId;
            var dietDto = _createDtoToDietDtoMapper.Map(createDto);
            if (dietDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_message.ErrorCreatingEntity);
            }

            dietDto.CreatedAt = DateTime.UtcNow;
            dietDto.UpdatedAt = dietDto.CreatedAt;

            return BusinessResponse<DietDto>.Ok(dietDto, $"{nameof(DietMappingValidationHandler)} passed");
        }
    }
}
