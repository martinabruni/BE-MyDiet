using BaseUtility;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Dtos.Requests;
using MyDiet.Core.Domain.Managers;
using MyDiet.Core.Domain.Options;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.Managers
{
    internal class DietManager : IDietManager
    {
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly IService<CoreUserDto, CoreUser, Guid> _userService;
        private readonly IMapper<CreateDietDto, DietDto> _createDtoToDietDtoMapper;
        private readonly IMapper<CreateDietRequest, CreateDietDto> _createRequestToCreateDtoMapper;
        private readonly DietManagerMessageOption _responseMessageOption;

        public DietManager(IService<CoreUserDto, CoreUser, Guid> userService, IService<DietDto, Diet, int> dietService, IMapper<CreateDietDto, DietDto> createDtoToDietDtoMapper, IMapper<CreateDietRequest, CreateDietDto> createRequestToCreateDtoMapper, DietManagerMessageOption responseMessageOption)
        {
            _userService = userService;
            _dietService = dietService;
            _createDtoToDietDtoMapper = createDtoToDietDtoMapper;
            _createRequestToCreateDtoMapper = createRequestToCreateDtoMapper;
            _responseMessageOption = responseMessageOption;
        }

        private static Guid? GetUserIdFromClaim(Claim? claim)
        {
            if (claim is null || !Guid.TryParse(claim.Value, out var userId))
            {
                return null;
            }
            return userId;
        }

        private static TRequest? IsValidRequest<TRequest>(TRequest request) where TRequest : class
        {
            if (request is null)
            {
                return null;
            }
            return request;
        }

        private static DietDto? ValidateRequestAndClaim<TRequest>(TRequest request, Claim? claim) where TRequest : class
        {
            var validRequest = IsValidRequest(request);
            if (validRequest is null)
            {
                return null;
            }
            var userId = GetUserIdFromClaim(claim);
            if (userId is null)
            {
                return null;
            }
            return new DietDto
            {
                Id = 0,
                UserId = (Guid)userId,
                Name = string.Empty
            };
        }

        public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
        {
            var validRequest = ValidateRequestAndClaim(request, userIdClaim);

            if (validRequest is null)
            {
                return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.InvalidRequest);
            }

            var userId = validRequest.UserId;
            var userRes = await _userService.GetByIdAsync(userId);

            if (userRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            var existingDietRes = await _dietService.FindAsync(d => d.Name == request.Name && d.UserId == userId);

            if (existingDietRes.Data is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_responseMessageOption.ErrorRetrievingEntities);
            }

            if (existingDietRes.Data.ToList().Count != 0)
            {
                return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.DietAlreadyExists);
            }

            var createDto = _createRequestToCreateDtoMapper.Map(request);
            createDto.UserId = userId;

            if (createDto is null)
            {
                return BusinessResponse<DietDto>.InternalServerError(_responseMessageOption.ErrorCreatingEntity);
            }

            return await _dietService.CreateAsync(_createDtoToDietDtoMapper.Map(createDto));
        }

        public async Task<BusinessResponse<DietDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            var userId = GetUserIdFromClaim(userIdClaim);

            if (userId is null)
            {
                return BusinessResponse<DietDto>.Unauthorize(_responseMessageOption.InvalidRequest);
            }

            var userRes = await _userService.GetByIdAsync((Guid)userId);

            if (userRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            var dietRes = await _dietService.GetByIdAsync(id);

            if (dietRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            if (dietRes.Data.UserId != userId)
            {
                return BusinessResponse<DietDto>.Unauthorize(_responseMessageOption.InvalidRequest);
            }

            return await _dietService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<DietDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            var userId = GetUserIdFromClaim(userIdClaim);
            if (userId is null)
            {
                return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.InvalidRequest);
            }

            var userRes = await _userService.GetByIdAsync((Guid)userId);

            if (userRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            var dietRes = await _dietService.GetByIdAsync(id);

            if (dietRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            if (dietRes.Data.UserId != userId)
            {
                return BusinessResponse<DietDto>.Unauthorize(_responseMessageOption.InvalidRequest);
            }

            return dietRes;
        }

        public async Task<BusinessResponse<IEnumerable<DietDto>>> GetByUserIdAsync(Claim? userIdClaim)
        {
            var userId = GetUserIdFromClaim(userIdClaim);
            if (userId is null)
            {
                return BusinessResponse<IEnumerable<DietDto>>.Unauthorize(_responseMessageOption.InvalidRequest);
            }
            return await _dietService.FindAsync(d => d.UserId == userId);
        }

        public async Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim)
        {
            var validRequest = ValidateRequestAndClaim(request, userIdClaim);

            if (validRequest is null)
            {
                return BusinessResponse<DietDto>.BadRequest(_responseMessageOption.InvalidRequest);
            }

            var userId = validRequest.UserId;
            var userRes = await _userService.GetByIdAsync(userId);

            if (userRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            var existingDietRes = await _dietService.GetByIdAsync(id);

            if (existingDietRes.Data is null)
            {
                return BusinessResponse<DietDto>.NotFound(_responseMessageOption.EntityNotFound);
            }

            if (existingDietRes.Data.UserId != userId)
            {
                return BusinessResponse<DietDto>.Unauthorize(_responseMessageOption.InvalidRequest);
            }

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

            newDto.UpdatedAt = DateTime.UtcNow;
            newDto.Id = id;
            return await _dietService.UpdateAsync(newDto);
        }
    }
}
