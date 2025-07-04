using BaseUtility;
using Microsoft.AspNetCore.Identity;
using Moq;
using MyDiet.Auth.Business.Managers;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Dtos.Claims;
using MyDiet.Auth.Domain.Dtos.Requests;
using MyDiet.Auth.Domain.Dtos.Responses;
using MyDiet.Auth.Domain.Enums;
using MyDiet.Auth.Domain.Managers;
using MyDiet.Auth.Domain.Options;
using MyDiet.Auth.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Shared.Test.UnitTests;

public class AuthManagerTests
{
    private readonly Mock<IService<AuthUserDto, AuthUser, Guid>> _authUserService = new();
    private readonly Mock<IMapper<UserRegistrationRequest, AuthUserDto>> _registrationRequestMapper = new();
    private readonly Mock<IMapper<AuthUserDto, UserRegistrationResponse>> _userResponseMapper = new();
    private readonly Mock<IMapper<AuthUserDto, UserClaims>> _userClaimsMapper = new();
    private readonly Mock<ITokenManager> _tokenManager = new();
    private readonly AuthManagerMessageOption _messageOptions = new();

    private AuthManager CreateManager() =>
        new AuthManager(
            _authUserService.Object,
            _registrationRequestMapper.Object,
            _userResponseMapper.Object,
            _tokenManager.Object,
            _userClaimsMapper.Object,
            _messageOptions
        );

    [Fact]
    public async Task RegisterUserAsync_ReturnsBadRequest_WhenRequestIsNull()
    {
        var manager = CreateManager();
        var result = await manager.RegisterUserAsync(null);

        Assert.Equal(BusinessCode.BadRequest, result.StatusCode);
        Assert.Equal(_messageOptions.InvalidRequest, result.Message);
        Assert.Null(result.Data);
    }

    [Theory]
    [InlineData("test@test.com", "pass")]
    public async Task RegisterUserAsync_ReturnsError_WhenFindAsyncReturnsNullData(string email, string password)
    {
        _authUserService
            .Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(BusinessResponse<IEnumerable<AuthUserDto>>.InternalServerError(_messageOptions.ErrorRetrievingEntities));

        var manager = CreateManager();
        var req = new UserRegistrationRequest
        {
            Email = email,
            Password = password
        };
        var result = await manager.RegisterUserAsync(req);

        Assert.Equal(BusinessCode.InternalServerError, result.StatusCode);
        Assert.Equal(_messageOptions.ErrorRetrievingEntities, result.Message);
        Assert.Null(result.Data);
    }

    [Theory]
    [InlineData("test@test.com", "pass", "hash")]
    public async Task RegisterUserAsync_ReturnsBadRequest_WhenUserAlreadyExists(string email, string password, string hash)
    {
        _authUserService
            .Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(BusinessResponse<IEnumerable<AuthUserDto>>.Ok(
                _messageOptions.EntitiesRetrievedSuccessfully,
                [
                    new AuthUserDto
                    {
                        Id = Guid.NewGuid(),
                        Email = email,
                        HashedPassword = hash,
                        Role = UserRole.User
                    }
                ]));

        var manager = CreateManager();
        var req = new UserRegistrationRequest
        {
            Email = email,
            Password = password
        };
        var result = await manager.RegisterUserAsync(req);

        Assert.Equal(BusinessCode.BadRequest, result.StatusCode);
        Assert.Equal(_messageOptions.UserAlreadyExists, result.Message);
        Assert.Null(result.Data);
    }

    [Theory]
    [InlineData("test@test.com", "pass", "hash")]
    public async Task RegisterUserAsync_ReturnsError_WhenCreateAsyncReturnsNullData(string email, string password, string hash)
    {
        _authUserService
            .Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(BusinessResponse<IEnumerable<AuthUserDto>>.Ok(
                _messageOptions.EntitiesRetrievedSuccessfully,
                []));

        _registrationRequestMapper
            .Setup(m => m.Map(It.IsAny<UserRegistrationRequest>()))
            .Returns(new AuthUserDto
            {
                Id = Guid.NewGuid(),
                Email = email,
                HashedPassword = hash,
                Role = UserRole.User
            });

        _authUserService
            .Setup(s => s.CreateAsync(It.IsAny<AuthUserDto>()))
            .ReturnsAsync(BusinessResponse<AuthUserDto>.InternalServerError(_messageOptions.UserRegistrationFailure));

        var manager = CreateManager();
        var req = new UserRegistrationRequest
        {
            Email = email,
            Password = password
        };
        var result = await manager.RegisterUserAsync(req);

        Assert.Equal(BusinessCode.InternalServerError, result.StatusCode);
        Assert.Equal(_messageOptions.UserRegistrationFailure, result.Message);
        Assert.Null(result.Data);
    }

    [Theory]
    [InlineData("test@test.com", "pass", "hash")]
    public async Task RegisterUserAsync_ReturnsCreated_WhenOk(string email, string password, string hash)
    {
        var userId = Guid.NewGuid();
        var userDto = new AuthUserDto
        {
            Id = userId,
            Email = email,
            HashedPassword = hash,
            Role = UserRole.User
        };
        var regResp = new UserRegistrationResponse
        {
            Id = userId,
            Email = email
        };

        _authUserService
            .Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(BusinessResponse<IEnumerable<AuthUserDto>>.Ok(
                _messageOptions.EntitiesRetrievedSuccessfully,
                []));

        _registrationRequestMapper
            .Setup(m => m.Map(It.IsAny<UserRegistrationRequest>()))
            .Returns(userDto);

        _authUserService
            .Setup(s => s.CreateAsync(It.IsAny<AuthUserDto>()))
            .ReturnsAsync(BusinessResponse<AuthUserDto>.Created(_messageOptions.UserRegistrationSuccess, userDto));

        _userResponseMapper
            .Setup(m => m.Map(It.IsAny<AuthUserDto>()))
            .Returns(regResp);

        var manager = CreateManager();
        var req = new UserRegistrationRequest
        {
            Email = email,
            Password = password
        };
        var result = await manager.RegisterUserAsync(req);

        Assert.Equal(BusinessCode.Created, result.StatusCode);
        Assert.Equal(_messageOptions.UserRegistrationSuccess, result.Message);
        Assert.Equal(regResp, result.Data);
    }

    [Theory]
    [InlineData("test@test.com", "pass")]
    public async Task LoginUserAsync_ReturnsError_WhenFindAsyncReturnsNullData(string email, string password)
    {
        _authUserService
            .Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(BusinessResponse<IEnumerable<AuthUserDto>>.InternalServerError(_messageOptions.ErrorRetrievingEntities));

        var manager = CreateManager();
        var req = new UserLoginRequest
        {
            Email = email,
            Password = password
        };
        var result = await manager.LoginUserAsync(req);

        Assert.Equal(BusinessCode.InternalServerError, result.StatusCode);
        Assert.Equal(_messageOptions.ErrorRetrievingEntities, result.Message);
        Assert.Null(result.Data);
    }

    [Theory]
    [InlineData("test@test.com", "pass")]
    public async Task LoginUserAsync_ReturnsBadRequest_WhenUserNotFound(string email, string password)
    {
        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>>
            {
                Data = [],
                StatusCode = BusinessCode.Ok
            });

        var manager = CreateManager();
        var req = new UserLoginRequest
        {
            Email = email,
            Password = password
        };
        var result = await manager.LoginUserAsync(req);

        Assert.Equal(BusinessCode.BadRequest, result.StatusCode);
        Assert.Equal(_messageOptions.UserNotRegistered, result.Message);
        Assert.Null(result.Data);
    }

    //TODO: adjust tests
    [Fact]
    public async Task LoginUserAsync_ReturnsUnauthorized_WhenPasswordInvalid()
    {
        var userDto = new AuthUserDto
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            HashedPassword = new PasswordHasher<AuthUserDto>().HashPassword(null, "otherpass"),
            Role = UserRole.User
        };
        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>> { Data = new[] { userDto }, StatusCode = BusinessCode.Ok });

        var manager = CreateManager();
        var req = new UserLoginRequest { Email = "test@test.com", Password = "wrongpass" };
        var result = await manager.LoginUserAsync(req);

        Assert.Equal(BusinessCode.Unauthorized, result.StatusCode);
        Assert.Equal(_messageOptions.InvalidCredentials, result.Message);
    }

    [Fact]
    public async Task LoginUserAsync_ReturnsTokenManagerResult_WhenPasswordValid()
    {
        var userDto = new AuthUserDto
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            HashedPassword = new PasswordHasher<AuthUserDto>().HashPassword(null, "pass"),
            Role = UserRole.User
        };
        var tokenResp = new TokenResponse { Token = "token", TokenExpiration = DateTime.UtcNow.AddHours(1) };

        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>> { Data = new[] { userDto }, StatusCode = BusinessCode.Ok });
        _userClaimsMapper.Setup(m => m.Map(It.IsAny<AuthUserDto>()))
            .Returns(new UserClaims { UserId = userDto.Id, Username = "test", Role = UserRole.User });
        _tokenManager.Setup(t => t.GenerateTokenAsync(It.IsAny<UserClaims>()))
            .ReturnsAsync(new BusinessResponse<TokenResponse> { Data = tokenResp, StatusCode = BusinessCode.Created, Message = "token created" });

        var manager = CreateManager();
        var req = new UserLoginRequest { Email = "test@test.com", Password = "pass" };
        var result = await manager.LoginUserAsync(req);

        Assert.Equal(BusinessCode.Created, result.StatusCode);
        Assert.Equal("token created", result.Message);
        Assert.Equal(tokenResp, result.Data);
    }

    [Fact]
    public async Task LogoutUserAsync_DelegatesToTokenManager()
    {
        var expected = new BusinessResponse<TokenResponse> { StatusCode = BusinessCode.Ok, Message = "revoked" };
        _tokenManager.Setup(t => t.RevokeTokenAsync("sometoken")).ReturnsAsync(expected);

        var manager = CreateManager();
        var result = await manager.LogoutUserAsync("sometoken");

        Assert.Equal(expected, result);
    }
}
