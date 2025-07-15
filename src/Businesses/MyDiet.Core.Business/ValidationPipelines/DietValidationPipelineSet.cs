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
                .AddHandler(new RequestValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message))
                .AddHandler(new DietNameUniquenessValidator(dietService, message, true))
                .AddHandler(new DietCreateRequestMapper(createRequestToDietDtoMapper, message));

            DeleteValidators
                .AddHandler(new RequestValidator<int, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<int, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message))
                .AddHandler(new DietIdExistenceValidator(dietService, message, false))
                .AddHandler(new DietIdOwnershipValidator(message, false));

            GetByIdValidators
                .AddHandler(new RequestValidator<int, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<int, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message))
                .AddHandler(new DietIdExistenceValidator(dietService, message, false, overrideContextData: true))
                .AddHandler(new DietIdOwnershipValidator(message, false));

            GetByUserIdValidators
                .AddHandler(new RequestValidator<Claim, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<Claim, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message));

            UpdateValidators
                .AddHandler(new RequestValidator<DietDto, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<DietDto, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message))
                .AddHandler(new DietExistenceValidator(dietService, message, false, true))
                .AddHandler(new DietOwnershipValidator(message, true))
                .AddHandler(new DietUpdateNameUniquenessValidator(dietService, message, true, false))
                .AddHandler(new DietUpdateRequestMapper(message));
        }
    }
}