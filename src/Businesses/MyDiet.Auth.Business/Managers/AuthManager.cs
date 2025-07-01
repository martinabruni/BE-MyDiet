using BaseUtility;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Managers
{
    internal class AuthManager : IAuthManager
    {
        private readonly IService<AuthUserDto, User, Guid> _authUserService;
        private readonly IMapper<UserRegistrationRequest, AuthUserDto> _userRegistrationMapper;
        private readonly IMapper<AuthUserDto, UserRegistrationResponse> _userResponseMapper;

        public AuthManager(IService<AuthUserDto, User, Guid> authUserService, IMapper<UserRegistrationRequest, AuthUserDto> userRegistrationMapper, IMapper<AuthUserDto, UserRegistrationResponse> userResponseMapper)
        {
            _authUserService = authUserService;
            _userRegistrationMapper = userRegistrationMapper;
            _userResponseMapper = userResponseMapper;
        }


        public async Task<BusinessResponse<UserRegistrationResponse>> RegisterUserAsync(UserRegistrationRequest userRegistrationDto)
        {
            if(userRegistrationDto is null)
            {
                return new BusinessResponse<UserRegistrationResponse>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "User registration request cannot be null."
                };
            }
            var existingUser = await _authUserService.FindAsync(u => u.Email == userRegistrationDto.Email);

            if(existingUser.Data is null)
            {
                return new BusinessResponse<UserRegistrationResponse>
                {
                    StatusCode = existingUser.StatusCode,
                    Message = existingUser.Message
                };
            }

            if (existingUser.Data.ToList().Count != 0)
            {
                return new BusinessResponse<UserRegistrationResponse>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "User with this email already exists."
                };
            }

            var newUser = _userRegistrationMapper.Map(userRegistrationDto);
            newUser.CreatedAt = DateTime.UtcNow;
            var registeredUser = await _authUserService.CreateAsync(newUser);

            if(registeredUser.Data is null)
            {
                return new BusinessResponse<UserRegistrationResponse>
                {
                    StatusCode = registeredUser.StatusCode,
                    Message = registeredUser.Message
                };
            }

            return new BusinessResponse<UserRegistrationResponse>
            {
                StatusCode = BusinessCode.Created,
                Data = _userResponseMapper.Map(registeredUser.Data),
                Message = "User registered successfully."
            };
        }
    }
}
