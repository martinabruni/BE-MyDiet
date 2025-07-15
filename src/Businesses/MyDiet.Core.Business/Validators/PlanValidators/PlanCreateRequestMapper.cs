using BaseUtility;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validators.PlanValidators
{
    internal class PlanCreateRequestMapper : BaseValidationHandler<CreatePlanRequest, PlanDto, CoreValidationContext<PlanDto, int>>
    {
        private readonly IMapper<CreatePlanRequest, PlanDto> _createDtoToPlanDtoMapper;
        private readonly ResponseMessage _message;

        public PlanCreateRequestMapper(IMapper<CreatePlanRequest, PlanDto> createDtoToPlanDtoMapper, ResponseMessage message)
        {
            _createDtoToPlanDtoMapper = createDtoToPlanDtoMapper;
            _message = message;
        }

        protected override Task<BusinessResponse<PlanDto>> ValidateAsync(CreatePlanRequest request, ContextProvider<CoreValidationContext<PlanDto, int>> context)
        {
            var createDto = _createDtoToPlanDtoMapper.Map(request);
            if (createDto is null)
            {
                return Task.FromResult(BusinessResponse<PlanDto>.InternalServerError(_message.ErrorCreatingEntity));
            }
            createDto.UserId = context.Context.UserId;
            createDto.CreatedAt = DateTime.UtcNow;
            createDto.UpdatedAt = createDto.CreatedAt;

            return Task.FromResult(BusinessResponse<PlanDto>.Ok(createDto, $"{nameof(PlanCreateRequestMapper)} passed"));
        }
    }
}
