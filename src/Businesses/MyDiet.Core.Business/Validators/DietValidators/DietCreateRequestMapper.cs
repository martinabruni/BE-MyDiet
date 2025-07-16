using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietCreateRequestMapper : BaseValidationHandler<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>
    {
        private readonly IMapper<CreateDietRequest, DietDto> _createRequestToDietDtoMapper;
        private readonly ResponseMessage _message;

        public DietCreateRequestMapper(IMapper<CreateDietRequest, DietDto> createRequestToDietDtoMapper, ResponseMessage message)
        {
            _createRequestToDietDtoMapper = createRequestToDietDtoMapper;
            _message = message;
        }

        protected override Task<BusinessResponse<DietDto>> ValidateAsync(CreateDietRequest request, ContextProvider<CoreValidationContext<DietDto, int>> context)
        {
            var dietDto = _createRequestToDietDtoMapper.Map(request);
            if (dietDto is null)
            {
                return Task.FromResult(BusinessResponse<DietDto>.InternalServerError(_message.ErrorMapping));
            }

            dietDto.UserId = context.Context.UserId;
            dietDto.CreatedAt = DateTime.UtcNow;
            dietDto.UpdatedAt = dietDto.CreatedAt;

            return Task.FromResult(BusinessResponse<DietDto>.Ok(dietDto, $"{this.GetType().Name} passed"));
        }
    }
}
