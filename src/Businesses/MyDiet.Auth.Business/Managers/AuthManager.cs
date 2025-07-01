using BaseUtility;
using Microsoft.AspNetCore.Identity;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Infrastructure.Models;

namespace MyDiet.Auth.Business.Managers
{
    internal class AuthManager : IAuthManager
    {
        private readonly IService<AuthUserDto, User, Guid> _authUserService;
        private readonly IMapper<UserRegistrationRequest, AuthUserDto> _registrationRequestMapper;
        private readonly IMapper<AuthUserDto, UserRegistrationResponse> _userResponseMapper;
        private readonly IMapper<AuthUserDto, UserClaims> _userClaimsMapper;
        private readonly ITokenManager _tokenManager;

        public AuthManager(IService<AuthUserDto, User, Guid> authUserService, IMapper<UserRegistrationRequest, AuthUserDto> registrationRequestMapper, IMapper<AuthUserDto, UserRegistrationResponse> registrationResponseMapper, ITokenManager tokenManager, IMapper<AuthUserDto, UserClaims> userClaimsMapper)
        {
            _authUserService = authUserService;
            _registrationRequestMapper = registrationRequestMapper;
            _userResponseMapper = registrationResponseMapper;
            _tokenManager = tokenManager;
            _userClaimsMapper = userClaimsMapper;
        }

        public async Task<BusinessResponse<UserRegistrationResponse>> RegisterUserAsync(UserRegistrationRequest request)
        {
            if (request is null)
            {
                return new BusinessResponse<UserRegistrationResponse>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "User registration request cannot be null."
                };
            }
            var existingUser = await _authUserService.FindAsync(u => u.Email == request.Email);

            if (existingUser.Data is null)
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

            var newUser = _registrationRequestMapper.Map(request);
            newUser.CreatedAt = DateTime.UtcNow;
            var registeredUser = await _authUserService.CreateAsync(newUser);

            if (registeredUser.Data is null)
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

        public async Task<BusinessResponse<TokenResponse>> LoginUserAsync(UserLoginRequest request)
        {
            if (request is null)
            {
                return new BusinessResponse<TokenResponse>
                {
                    StatusCode = BusinessCode.BadRequest,
                    Message = "User login request cannot be null."
                };
            }

            var existingUser = await _authUserService.FindAsync(u => u.Email == request.Email);

            if (existingUser.Data is null)
            {
                return new BusinessResponse<TokenResponse>
                {
                    StatusCode = existingUser.StatusCode,
                    Message = existingUser.Message
                };
            }

            var user = existingUser.Data.FirstOrDefault();

            if (user is null)
            {
                return new BusinessResponse<TokenResponse>
                {
                    StatusCode = BusinessCode.NotFound,
                    Message = "User not found."
                };
            }

            var verificationResult = new PasswordHasher<object>().VerifyHashedPassword(
                user.Id,
                user.HashedPassword,
                request.Password
            );

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return new BusinessResponse<TokenResponse>
                {
                    StatusCode = BusinessCode.Unauthorized,
                    Message = "Invalid password."
                };
            }

            var token = await _tokenManager.GenerateTokenAsync(_userClaimsMapper.Map(user));

            if (token.Data is null)
            {
                return new BusinessResponse<TokenResponse>
                {
                    StatusCode = token.StatusCode,
                    Message = token.Message
                };
            }

            return new BusinessResponse<TokenResponse>
            {
                StatusCode = BusinessCode.Created,
                Message = token.Message,
                Data = token.Data
            };
        }

        public async Task<BusinessResponse<TokenResponse>> LogoutUserAsync(string token)
        {
            return await _tokenManager.RevokeTokenAsync(token);
        }
    }
}
