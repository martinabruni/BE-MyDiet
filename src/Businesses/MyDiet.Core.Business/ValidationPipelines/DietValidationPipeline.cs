using BaseUtility;
using MyDiet.Core.Business.Validators;
using MyDiet.Core.Business.Validators.DietValidators;
using MyDiet.Core.Domain.Dtos.CoreUser;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Responses;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.ValidationPipelines
{
    internal class DietValidationPipeline
    {
        public ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>> CommonValidators { get; private set; }
        public ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>> CreateValidators { get; private set; }
        public ValidationPipeline<int, DietDto, CoreValidationContext<DietDto, int>> DeleteValidators { get; private set; }
        public ValidationPipeline<int, DietDto, CoreValidationContext<DietDto, int>> GetByIdValidators { get; private set; }
        public ValidationPipeline<Claim, DietDto, CoreValidationContext<DietDto, int>> GetByUserIdValidators { get; private set; }
        public ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>> UpdateValidators { get; private set; }

        public DietValidationPipeline(
            IService<CoreUserDto, CoreUser, Guid> userService,
            IService<DietDto, Diet, int> dietService,
            IMapper<CreateDietRequest, DietDto> createRequestToDietDtoMapper,
            DietMessage message)
        {
            InitializePipelines(userService, dietService, createRequestToDietDtoMapper, message);
        }

        private void InitializePipelines(
            IService<CoreUserDto, CoreUser, Guid> userService,
            IService<DietDto, Diet, int> dietService,
            IMapper<CreateDietRequest, DietDto> createRequestToDietDtoMapper,
            DietMessage message)
        {
            // Initialize common validators
            CommonValidators = new ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>();
            CommonValidators
                .AddHandler(new RequestValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message));

            // Initialize create validators
            CreateValidators = new ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>();
            CreateValidators = CommonValidators;
            CreateValidators
                .AddHandler(new DietNameUniquenessValidator(dietService, message, true))
                .AddHandler(new DietCreateRequestMapper(createRequestToDietDtoMapper, message));

            // Initialize delete validators
            DeleteValidators = new ValidationPipeline<int, DietDto, CoreValidationContext<DietDto, int>>();
            DeleteValidators
                .AddHandler(new RequestValidator<int, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<int, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message))
                .AddHandler(new DietIdExistenceValidator(dietService, message, false))
                .AddHandler(new DietIdOwnershipValidator(message));

            // Initialize get by id validators
            GetByIdValidators = new ValidationPipeline<int, DietDto, CoreValidationContext<DietDto, int>>();
            GetByIdValidators
                .AddHandler(new RequestValidator<int, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<int, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message))
                .AddHandler(new DietIdExistenceValidator(dietService, message, false))
                .AddHandler(new DietIdOwnershipValidator(message));

            // Initialize get by user id validators
            GetByUserIdValidators = new ValidationPipeline<Claim, DietDto, CoreValidationContext<DietDto, int>>();
            GetByUserIdValidators
                .AddHandler(new RequestValidator<Claim, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<Claim, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message));

            // Initialize update validators
            UpdateValidators = new ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>();
            UpdateValidators
                .AddHandler(new RequestValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message))
                .AddHandler(new DietNameUniquenessValidator(dietService, message, false))
                .AddHandler(new DietCreateOwnershipValidator(message));
        }
    }
}