using BaseUtility;
using MyDiet.Core.Business.Validators;
using MyDiet.Core.Business.Validators.DietValidators;
using MyDiet.Core.Business.Validators.PlanValidators;
using MyDiet.Core.Domain.Dtos.CoreUser;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.ValidationPipelines
{
    internal class PlanValidationPipelineSet
    {
        public ValidationPipeline<CreatePlanRequest, PlanDto, CoreValidationContext<PlanDto, int>> CreationValidators { get; private set; } = new();
        public ValidationPipeline<int, PlanDto, CoreValidationContext<PlanDto, int>> DeletionValidators { get; private set; } = new();
        public ValidationPipeline<int, PlanDto, CoreValidationContext<PlanDto, int>> GetByIdValidators { get; private set; } = new();
        public ValidationPipeline<Claim, PlanDto, CoreValidationContext<PlanDto, int>> GetByUserIdValidators { get; private set; } = new();
        public ValidationPipeline<PlanDto, PlanDto, CoreValidationContext<PlanDto, int>> UpdateValidators { get; private set; } = new();

        public PlanValidationPipelineSet(
            IService<CoreUserDto, CoreUser, Guid> userService,
            IService<PlanDto, Plan, int> planService,
            IService<DietDto, Diet, int> dietService,
            IMapper<CreatePlanRequest, PlanDto> createRequestToPlanDtoMapper,
            ResponseMessage message)
        {
            InitializePipelines(userService, planService, dietService, createRequestToPlanDtoMapper, message);
        }

        private void InitializePipelines(
            IService<CoreUserDto, CoreUser, Guid> userService,
            IService<PlanDto, Plan, int> planService,
            IService<DietDto, Diet, int> dietService,
            IMapper<CreatePlanRequest, PlanDto> createRequestToPlanDtoMapper,
            ResponseMessage message)
        {
            CreationValidators
                .AddHandlers([
                    new RequestValidator<CreatePlanRequest, PlanDto, CoreValidationContext<PlanDto, int>>(message),
                    new UserAuthenticationValidator<CreatePlanRequest, PlanDto, CoreValidationContext<PlanDto, int>, int>(userService, message),
                    new PlanCreateParentValidator(dietService, message),
                    new PlanNameUniquenessValidator(planService, message, true),
                    new PlanCreateRequestMapper(createRequestToPlanDtoMapper, message)
                ]);

            DeletionValidators
                .AddHandlers([
                    new RequestValidator<int, PlanDto, CoreValidationContext<PlanDto, int>>(message),
                    new UserAuthenticationValidator<int, PlanDto, CoreValidationContext<PlanDto, int>, int>(userService, message),
                    new PlanIdExistenceValidator(planService, message, false, overrideContextData : true),
                    new PlanIdParentValidator(dietService, message),
                ]);

            GetByIdValidators
                .AddHandlers([
                    new RequestValidator<int, PlanDto, CoreValidationContext<PlanDto, int>>(message),
                    new UserAuthenticationValidator<int, PlanDto, CoreValidationContext<PlanDto, int>, int>(userService, message),
                    new PlanIdExistenceValidator(planService, message, false, overrideContextData: true),
                    new PlanIdOwnershipValidator(message, false)
                ]);

            GetByUserIdValidators
                .AddHandlers([
                    new RequestValidator<Claim, PlanDto, CoreValidationContext<PlanDto, int>>(message),
                    new UserAuthenticationValidator<Claim, PlanDto, CoreValidationContext<PlanDto, int>, int>(userService, message),
                ]);

            UpdateValidators
                .AddHandlers([
                    new RequestValidator<PlanDto, PlanDto, CoreValidationContext<PlanDto, int>>(message),
                    new UserAuthenticationValidator<PlanDto, PlanDto, CoreValidationContext<PlanDto, int>, int>(userService, message),
                    new PlanExistenceValidator(planService, message, false, true),
                    new PlanOwnershipValidator(message, true),
                    new PlanUpdateNameUniquenessValidator(planService, message, true),
                    new PlanUpdateRequestMapper(message)
                ]);
        }
    }
}