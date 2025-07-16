using BaseUtility;
using MyDiet.Core.Business.Validators;
using MyDiet.Core.Business.Validators.DietValidators;
using MyDiet.Core.Domain.Dtos.CoreUser;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.ValidationPipelines
{
    //TODO: crea interfaccia
    internal class DietValidationPipelineSet
    {
        public ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>> CreateValidators { get; private set; } = new();
        public ValidationPipeline<int, DietDto, CoreValidationContext<DietDto, int>> DeleteValidators { get; private set; } = new();
        public ValidationPipeline<int, DietDto, CoreValidationContext<DietDto, int>> GetByIdValidators { get; private set; } = new();
        public ValidationPipeline<Claim, DietDto, CoreValidationContext<DietDto, int>> GetByUserIdValidators { get; private set; } = new();
        public ValidationPipeline<DietDto, DietDto, CoreValidationContext<DietDto, int>> UpdateValidators { get; private set; } = new();

        public DietValidationPipelineSet(
            IService<CoreUserDto, CoreUser, Guid> userService,
            IService<DietDto, Diet, int> dietService,
            IMapper<CreateDietRequest, DietDto> createRequestToDietDtoMapper,
            ResponseMessage message)
        {
            InitializePipelines(userService, dietService, createRequestToDietDtoMapper, message);
        }

        private void InitializePipelines(
            IService<CoreUserDto, CoreUser, Guid> userService,
            IService<DietDto, Diet, int> dietService,
            IMapper<CreateDietRequest, DietDto> createRequestToDietDtoMapper,
            ResponseMessage message)
        {
            CreateValidators
                .AddHandlers([
                    new RequestValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>(message),
                    new UserAuthenticationValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message),
                    new DietNameUniquenessValidator(dietService, message, true),
                    new DietCreateRequestMapper(createRequestToDietDtoMapper, message)
                ]);

            DeleteValidators
                .AddHandlers([
                    new RequestValidator<int, DietDto, CoreValidationContext<DietDto, int>>(message),
                    new UserAuthenticationValidator<int, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message),
                    new DietIdExistenceValidator(dietService, message, false),
                    new DietIdOwnershipValidator(message, false)
                ]);

            GetByIdValidators
                .AddHandlers([
                    new RequestValidator<int, DietDto, CoreValidationContext<DietDto, int>>(message),
                    new UserAuthenticationValidator<int, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message),
                    new DietIdExistenceValidator(dietService, message, false, overrideContextData: true),
                    new DietIdOwnershipValidator(message, false)
                ]);

            GetByUserIdValidators
                .AddHandlers([
                    new RequestValidator<Claim, DietDto, CoreValidationContext<DietDto, int>>(message),
                    new UserAuthenticationValidator<Claim, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message)
                ]);

            UpdateValidators
                .AddHandlers([
                    new RequestValidator<DietDto, DietDto, CoreValidationContext<DietDto, int>>(message),
                    new UserAuthenticationValidator<DietDto, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message),
                    new DietExistenceValidator(dietService, message, false, true),
                    new DietOwnershipValidator(message, true),
                    new DietUpdateNameUniquenessValidator(dietService, message, true, false),
                    new DietUpdateRequestMapper(message)
                ]);
        }
    }
}