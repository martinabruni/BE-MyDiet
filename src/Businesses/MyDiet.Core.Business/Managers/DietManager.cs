using BaseUtility;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Dtos.ForeignKeys;
using MyDiet.Core.Domain.Dtos.Requests;
using MyDiet.Core.Domain.Managers;
using MyDiet.Core.Domain.Options;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.Managers
{
    internal class DietManager : BaseManager<DietDto, CreateDietRequest, int>, IManager<DietDto, CreateDietRequest, int>
    {
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly IMapper<CreateDietDto, DietDto> _createDtoToDietDtoMapper;
        private readonly IMapper<CreateDietRequest, CreateDietDto> _createRequestToCreateDtoMapper;
        private readonly DietMessageOption _responseMessageOption;

        public DietManager(IService<CoreUserDto, CoreUser, Guid> userService, IService<DietDto, Diet, int> dietService, IMapper<CreateDietDto, DietDto> createDtoToDietDtoMapper, IMapper<CreateDietRequest, CreateDietDto> createRequestToCreateDtoMapper, DietMessageOption responseMessageOption)
            : base(userService)
        {
            _dietService = dietService;
            _createDtoToDietDtoMapper = createDtoToDietDtoMapper;
            _createRequestToCreateDtoMapper = createRequestToCreateDtoMapper;
            _responseMessageOption = responseMessageOption;
        }

        public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
        {
            // Use template method for user and request validation
            var (userId, validationError) = await ValidateUserAndRequestAsync(
                request, 
                userIdClaim, 
                _responseMessageOption.InvalidRequest,
                _responseMessageOption.EntityNotFound);

            if (validationError != null)
            {
                return validationError;
            }

            // Check if diet with same name already exists for user
            var existingDietRes = await _dietService.FindAsync(d => d.Name == request.Name && d.UserId == userId);

            if (existingDietRes.Data is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_responseMessageOption.ErrorRetrievingEntities);
            }

            if (existingDietRes.Data.ToList().Count != 0)
            {
                return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.DietAlreadyExists);
            }

            // Map request to DTO
            var createDto = _createRequestToCreateDtoMapper.Map(request);
            if (createDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_responseMessageOption.ErrorCreatingEntity);
            }

            createDto.UserId = userId;
            var dietDto = _createDtoToDietDtoMapper.Map(createDto);
            if (dietDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_responseMessageOption.ErrorCreatingEntity);
            }

            // Apply creation timestamps using template method
            ApplyCreationTimestamps(dietDto);

            return await _dietService.CreateAsync(dietDto);
        }

        public async Task<BusinessResponse<DietDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            // Use template method for user validation
            var (userId, validationError) = await ValidateUserAsync(
                userIdClaim,
                _responseMessageOption.InvalidRequest,
                _responseMessageOption.EntityNotFound);

            if (validationError != null)
            {
                return validationError;
            }

            // Get diet entity and validate existence
            var dietRes = await ValidateEntityExistsAsync(_dietService, id, _responseMessageOption.EntityNotFound);
            if (dietRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            // Validate ownership using template method
            var ownershipError = ValidateOwnership(userId, dietRes.Data.UserId, _responseMessageOption.InvalidRequest);
            if (ownershipError != null)
            {
                return ownershipError;
            }

            return await _dietService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<DietDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            // Use template method for user validation
            var (userId, validationError) = await ValidateUserAsync(
                userIdClaim,
                _responseMessageOption.InvalidRequest,
                _responseMessageOption.EntityNotFound);

            if (validationError != null)
            {
                return validationError;
            }

            // Get diet entity and validate existence
            var dietRes = await ValidateEntityExistsAsync(_dietService, id, _responseMessageOption.EntityNotFound);
            if (dietRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            // Validate ownership using template method
            var ownershipError = ValidateOwnership(userId, dietRes.Data.UserId, _responseMessageOption.InvalidRequest);
            if (ownershipError != null)
            {
                return ownershipError;
            }

            return dietRes;
        }

        public async Task<BusinessResponse<IEnumerable<DietDto>>> GetByUserIdAsync(Claim? userIdClaim)
        {
            // Use template method for user validation
            var userId = ValidateUserClaim(userIdClaim);
            if (userId is null)
            {
                return BusinessResponse<IEnumerable<DietDto>>.Unauthorize(_responseMessageOption.InvalidRequest);
            }
            
            return await _dietService.FindAsync(d => d.UserId == userId);
        }

        public async Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim)
        {
            // Use template method for user and request validation
            var (userId, validationError) = await ValidateUserAndRequestAsync(
                request,
                userIdClaim,
                _responseMessageOption.InvalidRequest,
                _responseMessageOption.EntityNotFound);

            if (validationError != null)
            {
                return validationError;
            }

            // Get existing diet and validate existence
            var existingDietRes = await ValidateEntityExistsAsync(_dietService, id, _responseMessageOption.EntityNotFound);
            if (existingDietRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            // Validate ownership using template method
            var ownershipError = ValidateOwnership(userId, existingDietRes.Data.UserId, _responseMessageOption.InvalidRequest);
            if (ownershipError != null)
            {
                return ownershipError;
            }

            // Map request to DTO
            var createDto = _createRequestToCreateDtoMapper.Map(request);
            createDto.UserId = userId;

            if (createDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_responseMessageOption.ErrorUpdatingEntity);
            }

            var newDto = _createDtoToDietDtoMapper.Map(createDto);
            if (newDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_responseMessageOption.ErrorUpdatingEntity);
            }

            // Apply update timestamps using template method
            ApplyUpdateTimestamps(newDto, existingDietRes.Data.CreatedAt);
            newDto.Id = id;
            
            return await _dietService.UpdateAsync(newDto);
        }
    }
}
