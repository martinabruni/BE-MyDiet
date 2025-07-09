using BaseUtility;
using MyDiet.Core.Business.Validation;
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
        private readonly IMapper<CreateDietDto, DietDto> _createDtoToDietDtoMapper;
        private readonly IMapper<CreateDietRequest, CreateDietDto> _createRequestToCreateDtoMapper;
        private readonly DietMessage _message;
        private readonly ValidationPipeline<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>> _validationPipeline = new();

        public DietManager(IService<CoreUserDto, CoreUser, Guid> userService, IService<DietDto, Diet, int> dietService, IMapper<CreateDietDto, DietDto> createDtoToDietDtoMapper, IMapper<CreateDietRequest, CreateDietDto> createRequestToCreateDtoMapper, DietMessage message)
        {
            _userService = userService;
            _dietService = dietService;
            _createDtoToDietDtoMapper = createDtoToDietDtoMapper;
            _createRequestToCreateDtoMapper = createRequestToCreateDtoMapper;
            _message = message;
            _validationPipeline
                .AddHandler(new RequestValidationHandler<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>>(message))
                .AddHandler(new UserAuthenticationValidationHandler<CreateDietRequest, DietDto, CoreValidationContext<DietDto, int>, int>(userService, message))
                .AddHandler(new DietExistenceValidationHandler(dietService, message))
                .AddHandler(new DietMappingValidationHandler(createRequestToCreateDtoMapper, message, createDtoToDietDtoMapper));
        }

        public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
        {
            _validationContext.Context = new CoreValidationContext<DietDto, int>
            {
                UserClaim = userIdClaim
            };

            var validationRes = await _validationPipeline.ValidateAsync(request, _validationContext);
            var dietDto = validationRes.Data;
            if (dietDto is null)
            {
                return validationRes;
            }

            return await _dietService.CreateAsync(dietDto);
        }

        public async Task<BusinessResponse<DietDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            var userId = ValidateUserClaim(userIdClaim);

            if (userId is null)
            {
                return BusinessResponse<DietDto>.Unauthorize(_message.InvalidRequest);
            }

            var userRes = await _userService.GetByIdAsync((Guid)userId);

            if (userRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_message.EntityNotFound);
            }

            var dietRes = await _dietService.GetByIdAsync(id);

            if (dietRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_message.EntityNotFound);
            }

            if (dietRes.Data.UserId != userId)
            {
                return BusinessResponse<DietDto>.Unauthorize(_message.InvalidRequest);
            }

            return await _dietService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<DietDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            var userId = ValidateUserClaim(userIdClaim);
            if (userId is null)
            {
                return BusinessResponse<DietDto>.BadRequest(_message.InvalidRequest);
            }

            var userRes = await _userService.GetByIdAsync((Guid)userId);

            if (userRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_message.EntityNotFound);
            }

            var dietRes = await _dietService.GetByIdAsync(id);

            if (dietRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_message.EntityNotFound);
            }

            if (dietRes.Data.UserId != userId)
            {
                return BusinessResponse<DietDto>.Unauthorize(_message.InvalidRequest);
            }

            return dietRes;
        }

        public async Task<BusinessResponse<IEnumerable<DietDto>>> GetByUserIdAsync(Claim? userIdClaim)
        {
            var userId = ValidateUserClaim(userIdClaim);
            if (userId is null)
            {
                return BusinessResponse<IEnumerable<DietDto>>.Unauthorize(_message.InvalidRequest);
            }
            return await _dietService.FindAsync(d => d.UserId == userId);
        }

        public async Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim)
        {
            var validRequest = ValidateAndGetUserId(request, userIdClaim);

            if (validRequest is null)
            {
                return BusinessResponse<DietDto>.BadRequest(_message.InvalidRequest);
            }

            var userId = (Guid)validRequest;
            var userRes = await _userService.GetByIdAsync(userId);

            if (userRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_message.EntityNotFound);
            }

            var existingDietRes = await _dietService.GetByIdAsync(id);

            if (existingDietRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_message.EntityNotFound);
            }

            if (existingDietRes.Data.UserId != userId)
            {
                return BusinessResponse<DietDto>.Unauthorize(_message.InvalidRequest);
            }

            var createDto = _createRequestToCreateDtoMapper.Map(request);
            createDto.UserId = userId;

            if (createDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_message.ErrorUpdatingEntity);
            }

            var newDto = _createDtoToDietDtoMapper.Map(createDto);

            if (newDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_message.ErrorUpdatingEntity);
            }

            newDto.CreatedAt = existingDietRes.Data.CreatedAt;
            newDto.UpdatedAt = DateTime.UtcNow;
            newDto.Id = id;
            return await _dietService.UpdateAsync(newDto);
        }
    }
}
