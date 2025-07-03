using BaseUtility;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Dtos.Requests;
using MyDiet.Core.Domain.Managers;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Shared.Business.Managers
{
    internal class DietManager : IDietManager
    {
        private readonly IService<DietDto, Diet, int> _dietService;
        private readonly IService<CoreUserDto, CoreUser, Guid> _userService;
        private readonly IMapper<CreateDietDto, DietDto> _createDtoToDietDtoMapper;
        private readonly IMapper<CreateDietRequest, CreateDietDto> _createRequestToCreateDtoMapper;

        public DietManager(IService<CoreUserDto, CoreUser, Guid> userService, IService<DietDto, Diet, int> dietService, IMapper<CreateDietDto, DietDto> createDtoToDietDtoMapper, IMapper<CreateDietRequest, CreateDietDto> createRequestToCreateDtoMapper)
        {
            _userService = userService;
            _dietService = dietService;
            _createDtoToDietDtoMapper = createDtoToDietDtoMapper;
            _createRequestToCreateDtoMapper = createRequestToCreateDtoMapper;
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

        private static BusinessResponse<DietDto> ValidateRequestAndClaim<TRequest>(TRequest request, Claim? claim) where TRequest : class
        {
            var validRequest = IsValidRequest(request);
            if (validRequest is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "Request cannot be null."
                };
            }
            var userId = GetUserIdFromClaim(claim);
            if (userId is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "Invalid claim value."
                };
            }
            return new BusinessResponse<DietDto>
            {
                StatusCode = BusinessCode.Ok,
                Data = new DietDto
                {
                    Id = 0,
                    UserId = (Guid)userId,
                    Name = string.Empty
                }
            };
        }

        public async Task<BusinessResponse<DietDto>> CreateAsync(CreateDietRequest request, Claim? userIdClaim)
        {
            var isValidRes = ValidateRequestAndClaim(request, userIdClaim);

            if (isValidRes.Data is null)
            {
                return isValidRes;
            }

            var userId = isValidRes.Data.UserId;
            var userRes = await _userService.GetByIdAsync(userId);

            if (userRes.Data is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = userRes.StatusCode,
                    Message = userRes.Message
                };
            }

            var existingDietRes = await _dietService.FindAsync(d => d.Name == request.Name && d.UserId == userId);

            if (existingDietRes.Data is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = existingDietRes.StatusCode,
                    Message = existingDietRes.Message
                };
            }

            if (existingDietRes.Data.ToList().Count != 0)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "A diet with the same name already exists for this user."
                };
            }

            var createDto = _createRequestToCreateDtoMapper.Map(request);
            createDto.UserId = userId;

            if (createDto is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.InternalServerError,
                    Message = "Failed to map request."
                };
            }

            return await _dietService.CreateAsync(_createDtoToDietDtoMapper.Map(createDto));
        }

        public async Task<BusinessResponse<DietDto>> DeleteAsync(int id, Claim? userIdClaim)
        {
            var userId = GetUserIdFromClaim(userIdClaim);

            if (userId is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.Unauthorized,
                    Message = "Required claim is missing."
                };
            }

            var userRes = await _userService.GetByIdAsync((Guid)userId);

            if (userRes.Data is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = userRes.StatusCode,
                    Message = userRes.Message
                };
            }

            var dietRes = await _dietService.GetByIdAsync(id);

            if (dietRes.Data is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = dietRes.StatusCode,
                    Message = dietRes.Message
                };
            }

            if (dietRes.Data.UserId != userId)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.Unauthorized,
                    Message = "You are not authorized to delete this diet."
                };
            }

            return await _dietService.DeleteAsync(id);
        }

        public async Task<BusinessResponse<DietDto>> GetByIdAsync(int id, Claim? userIdClaim)
        {
            var userId = GetUserIdFromClaim(userIdClaim);
            if (userId is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.Unauthorized,
                    Message = "Required claim is missing."
                };
            }

            var userRes = await _userService.GetByIdAsync((Guid)userId);
            
            if (userRes.Data is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = userRes.StatusCode,
                    Message = userRes.Message
                };
            }

            var dietRes = await _dietService.GetByIdAsync(id);

            if (dietRes.Data is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = dietRes.StatusCode,
                    Message = dietRes.Message
                };
            }

            if (dietRes.Data.UserId != userId)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.Unauthorized,
                    Message = "You are not authorized to access this diet."
                };
            }

            return dietRes;
        }

        public async Task<BusinessResponse<IEnumerable<DietDto>>> GetByUserIdAsync(Claim? userIdClaim)
        {
            var userId = GetUserIdFromClaim(userIdClaim);
            if (userId is null)
            {
                return new BusinessResponse<IEnumerable<DietDto>>
                {
                    StatusCode = BusinessCode.Unauthorized,
                    Message = "Required claim is missing."
                };
            }
            return await _dietService.FindAsync(d => d.UserId == userId);
        }

        public async Task<BusinessResponse<DietDto>> UpdateAsync(CreateDietRequest request, int id, Claim? userIdClaim)
        {
            var isValidRes = ValidateRequestAndClaim(request, userIdClaim);

            if (isValidRes.Data is null)
            {
                return isValidRes;
            }

            var userId = isValidRes.Data.UserId;
            var userRes = await _userService.GetByIdAsync(userId);

            if (userRes.Data is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = userRes.StatusCode,
                    Message = userRes.Message
                };
            }

            var existingDietRes = await _dietService.GetByIdAsync(id);

            if (existingDietRes.Data is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = existingDietRes.StatusCode,
                    Message = existingDietRes.Message
                };
            }

            if (existingDietRes.Data.UserId != userId)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.Unauthorized,
                    Message = "You are not authorized to update this diet."
                };
            }

            var createDto = _createRequestToCreateDtoMapper.Map(request);
            createDto.UserId = userId;

            if (createDto is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.InternalServerError,
                    Message = "Failed to map request."
                };
            }

            var newDto = _createDtoToDietDtoMapper.Map(createDto);

            if (newDto is null)
            {
                return new BusinessResponse<DietDto>
                {
                    StatusCode = BusinessCode.InternalServerError,
                    Message = "Failed to map request."
                };
            }

            newDto.UpdatedAt = DateTime.UtcNow;
            newDto.Id = id;
            return await _dietService.UpdateAsync(newDto);
        }
    }
}
