using BaseUtility;
using Microsoft.AspNetCore.Identity;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Infrastructure.Models;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("MyDiet.Shared.Test")]

namespace MyDiet.Auth.Business.Managers
{
    internal class AuthManager : IAuthManager
    {
        private readonly IService<AuthUserDto, AuthUser, Guid> _authUserService;
        private readonly IMapper<UserRegistrationRequest, AuthUserDto> _registrationRequestMapper;
        private readonly IMapper<AuthUserDto, UserRegistrationResponse> _userResponseMapper;
        private readonly IMapper<AuthUserDto, UserClaims> _userClaimsMapper;
        private readonly ITokenManager _tokenManager;
        private readonly AuthManagerMessageOption _responseMessageOptions;

        public AuthManager(IService<AuthUserDto, AuthUser, Guid> authUserService, IMapper<UserRegistrationRequest, AuthUserDto> registrationRequestMapper, IMapper<AuthUserDto, UserRegistrationResponse> registrationResponseMapper, ITokenManager tokenManager, IMapper<AuthUserDto, UserClaims> userClaimsMapper, AuthManagerMessageOption responseMessageOptions)
        {
            _authUserService = authUserService;
            _registrationRequestMapper = registrationRequestMapper;
            _userResponseMapper = registrationResponseMapper;
            _tokenManager = tokenManager;
            _userClaimsMapper = userClaimsMapper;
            _responseMessageOptions = responseMessageOptions;
        }

        public async Task<BusinessResponse<UserRegistrationResponse>> RegisterUserAsync(UserRegistrationRequest request)
        {
            if (request is null)
            {
                return BusinessResponse<UserRegistrationResponse>.BadRequest(_responseMessageOptions.InvalidRequest);
            }
            var existingUser = await _authUserService.FindAsync(u => u.Email == request.Email);

            if (existingUser.Data is null)
            {
                return BusinessResponse<UserRegistrationResponse>.InternalServerError(_responseMessageOptions.ErrorRetrievingEntities);
            }

            if (existingUser.Data.ToList().Count != 0)
            {
                return BusinessResponse<UserRegistrationResponse>.BadRequest(_responseMessageOptions.UserAlreadyExists);
            }

            var newUser = _registrationRequestMapper.Map(request);
            newUser.CreatedAt = DateTime.UtcNow;
            var registeredUser = await _authUserService.CreateAsync(newUser);

            if (registeredUser.Data is null)
            {
                return BusinessResponse<UserRegistrationResponse>.InternalServerError(_responseMessageOptions.UserRegistrationFailure);
            }

            return BusinessResponse<UserRegistrationResponse>.Created(_responseMessageOptions.UserRegistrationSuccess, _userResponseMapper.Map(registeredUser.Data));
        }

        public async Task<BusinessResponse<TokenResponse>> LoginUserAsync(UserLoginRequest request)
        {
            if (request is null)
            {
                return BusinessResponse<TokenResponse>.BadRequest(_responseMessageOptions.InvalidRequest);
            }

            var existingUser = await _authUserService.FindAsync(u => u.Email == request.Email);

            if (existingUser.Data is null)
            {
                return BusinessResponse<TokenResponse>.InternalServerError(_responseMessageOptions.ErrorRetrievingEntities);
            }

            var user = existingUser.Data.FirstOrDefault();

            if (user is null)
            {
                return  BusinessResponse<TokenResponse>.BadRequest(_responseMessageOptions.UserNotRegistered);
            }

            var verificationResult = new PasswordHasher<AuthUserDto>().VerifyHashedPassword(
                user,
                user.HashedPassword,
                request.Password
            );

            if (verificationResult is PasswordVerificationResult.Failed)
            {
                return BusinessResponse<TokenResponse>.Unauthorize(_responseMessageOptions.InvalidCredentials);
            }

            return await _tokenManager.GenerateTokenAsync(_userClaimsMapper.Map(user));
        }

        public async Task<BusinessResponse<TokenResponse>> LogoutUserAsync(string token)
        {
            return await _tokenManager.RevokeTokenAsync(token);
        }
    }
}
