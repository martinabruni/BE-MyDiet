using BaseUtility;
using MyDiet.Core.Business.Validators;
using MyDiet.Core.Domain.Dtos.CoreUser;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Managers;
using MyDiet.Core.Domain.Responses;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.Managers
{
    internal class DietManager : BaseManager<CreateDietRequest>, IManager<DietDto, CreateDietRequest, int>
    {
        private ValidationContext<CoreValidationContext<DietDto, int>> _validationContext = new() { Context = new() };
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly IService<CoreUserDto, CoreUser, Guid> _userService;
        private readonly IMapper<CreateDietRequest, DietDto> _createRequestToDietDtoMapper;
        private readonly DietMessage _message;
        private readonly ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>> _commonValidators = new();
        private readonly ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>> _createValidators = new();
        private readonly ValidationPipeline<int, DietDto, CoreValidationContext<DietDto, int>> _deleteValidators = new();
        private readonly ValidationPipeline<int, DietDto, CoreValidationContext<DietDto, int>> _getByIdValidators = new();
        private readonly ValidationPipeline<Claim, DietDto, CoreValidationContext<DietDto, int>> _getByUserIdValidators = new();
        private readonly ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>> _updateValidators = new();

        public DietManager(IService<CoreUserDto, CoreUser, Guid> userService, IService<DietDto, Diet, int> dietService, IMapper<CreateDietRequest, DietDto> createRequestToDietDtoMapper, DietMessage message)
        {
            _userService = userService;
            _dietService = dietService;
            _createRequestToDietDtoMapper = createRequestToDietDtoMapper;
            _message = message;
            _commonValidators
                .AddHandler(new RequestValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>(_message))
                .AddHandler(new UserAuthenticationValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>, int>(_userService, _message));
            _createValidators = _commonValidators;
            _createValidators
                .AddHandler(new CreateDietExistenceValidator(dietService, _message, true))
                .AddHandler(new CreateDietMappingValidator(_createRequestToDietDtoMapper, _message));
            _deleteValidators
                .AddHandler(new RequestValidator<int, DietDto, CoreValidationContext<DietDto, int>>(_message))
                .AddHandler(new UserAuthenticationValidator<int, DietDto, CoreValidationContext<DietDto, int>, int>(userService, _message))
                .AddHandler(new DietExistenceByIdValidator(dietService, _message, false))
                .AddHandler(new IdAuthorizationValidator(_message));
            _getByIdValidators
                .AddHandler(new RequestValidator<int, DietDto, CoreValidationContext<DietDto, int>>(_message))
                .AddHandler(new UserAuthenticationValidator<int, DietDto, CoreValidationContext<DietDto, int>, int>(userService, _message))
                .AddHandler(new DietExistenceByIdValidator(dietService, _message, false))
                .AddHandler(new IdAuthorizationValidator(_message));
            _getByUserIdValidators
                .AddHandler(new RequestValidator<Claim, DietDto, CoreValidationContext<DietDto, int>>(_message))
                .AddHandler(new UserAuthenticationValidator<Claim, DietDto, CoreValidationContext<DietDto, int>, int>(userService, _message));
            _updateValidators
                .AddHandler(new RequestValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>(_message))
                .AddHandler(new UserAuthenticationValidator<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>, int>(userService, _message))
                .AddHandler(new CreateDietExistenceValidator(dietService, _message, false))
                .AddHandler(new CreateAuthorizationValidator(_message));

        }

        public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
        {
            _validationContext.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _commonValidators.ValidateAsync(request, _validationContext);
            var dietDto = validationRes.Data;

            if (dietDto is null)
            {
                return validationRes;
            }

            return await _dietService.CreateAsync(dietDto);
        }

        public async Task<BusinessResponse<DietDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            _validationContext.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _deleteValidators.ValidateAsync(id, _validationContext);
            return await _dietService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<DietDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            _validationContext.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _getByIdValidators.ValidateAsync(id, _validationContext);

            return await _dietService.GetByIdAsync(id);
        }

        public async Task<BusinessResponse<IEnumerable<DietDto>>> GetByUserIdAsync(Claim? userIdClaim)
        {
            _validationContext.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _getByUserIdValidators.ValidateAsync(userIdClaim, _validationContext);

            return await _dietService.FindAsync(d => d.UserId == validationRes.Data.UserId);
        }

        public async Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim)
        {
            _validationContext.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _updateValidators.ValidateAsync(request, _validationContext);

            if (validationRes.Data is null)
            {
                return validationRes;
            }

            return await _dietService.UpdateAsync(validationRes.Data);
        }
    }
}
