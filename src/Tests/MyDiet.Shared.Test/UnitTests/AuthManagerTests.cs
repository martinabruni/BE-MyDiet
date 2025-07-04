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

    private AuthManager CreateManager() =>
        new AuthManager(
            _authUserService.Object,
            _registrationRequestMapper.Object,
            _userResponseMapper.Object,
            _tokenManager.Object,
            _userClaimsMapper.Object
        );

    [Fact]
    public async Task RegisterUserAsync_ReturnsBadRequest_WhenRequestIsNull()
    {
        var manager = CreateManager();
        var result = await manager.RegisterUserAsync(null);

        Assert.Equal(BusinessCode.BadRequest, result.StatusCode);
        Assert.Equal("User registration request cannot be null.", result.Message);
    }

    [Theory]
    [InlineData("test@test.com", "pass", BusinessCode.InternalServerError, "error")]
    public async Task RegisterUserAsync_ReturnsError_WhenFindAsyncReturnsNullData(string email, string password, BusinessCode code, string message)
    {
        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>> { Data = null, StatusCode = code, Message = message });

        var manager = CreateManager();
        var req = new UserRegistrationRequest { Email = email, Password = password };
        var result = await manager.RegisterUserAsync(req);

        Assert.Equal(code, result.StatusCode);
        Assert.Equal(message, result.Message);
    }

    [Theory]
    [InlineData("test@test.com", "pass")]
    public async Task RegisterUserAsync_ReturnsBadRequest_WhenUserAlreadyExists(string email, string password)
    {
        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>>
            {
                Data = new[]
                {
                    new AuthUserDto
                    {
                        Id = Guid.NewGuid(),
                        Email = email,
                        HashedPassword = "hash",
                        Role = UserRole.User
                    }
                },
                StatusCode = BusinessCode.Ok
            });

        var manager = CreateManager();
        var req = new UserRegistrationRequest { Email = email, Password = password };
        var result = await manager.RegisterUserAsync(req);

        Assert.Equal(BusinessCode.BadRequest, result.StatusCode);
        Assert.Equal("User with this email already exists.", result.Message);
    }

    [Theory]
    [InlineData("test@test.com", "pass", BusinessCode.InternalServerError, "create error")]
    public async Task RegisterUserAsync_ReturnsError_WhenCreateAsyncReturnsNullData(string email, string password, BusinessCode code, string message)
    {
        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>>
            {
                Data = Enumerable.Empty<AuthUserDto>(),
                StatusCode = BusinessCode.Ok
            });
        _registrationRequestMapper.Setup(m => m.Map(It.IsAny<UserRegistrationRequest>()))
            .Returns(new AuthUserDto
            {
                Id = Guid.NewGuid(),
                Email = email,
                HashedPassword = "hash",
                Role = UserRole.User
            });
        _authUserService.Setup(s => s.CreateAsync(It.IsAny<AuthUserDto>()))
            .ReturnsAsync(new BusinessResponse<AuthUserDto>
            {
                Data = null,
                StatusCode = code,
                Message = message
            });

        var manager = CreateManager();
        var req = new UserRegistrationRequest { Email = email, Password = password };
        var result = await manager.RegisterUserAsync(req);

        Assert.Equal(code, result.StatusCode);
        Assert.Equal(message, result.Message);
    }

    [Theory]
    [InlineData("test@test.com", "pass")]
    public async Task RegisterUserAsync_ReturnsCreated_WhenOk(string email, string password)
    {
        var userDto = new AuthUserDto
        {
            Id = Guid.NewGuid(),
            Email = email,
            HashedPassword = "hash",
            Role = UserRole.User
        };
        var regResp = new UserRegistrationResponse { Id = Guid.NewGuid(), Email = email };

        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>>
            {
                Data = Enumerable.Empty<AuthUserDto>(),
                StatusCode = BusinessCode.Ok
            });
        _registrationRequestMapper.Setup(m => m.Map(It.IsAny<UserRegistrationRequest>()))
            .Returns(userDto);
        _authUserService.Setup(s => s.CreateAsync(It.IsAny<AuthUserDto>()))
            .ReturnsAsync(new BusinessResponse<AuthUserDto>
            {
                Data = userDto,
                StatusCode = BusinessCode.Created
            });
        _userResponseMapper.Setup(m => m.Map(It.IsAny<AuthUserDto>()))
            .Returns(regResp);

        var manager = CreateManager();
        var req = new UserRegistrationRequest { Email = email, Password = password };
        var result = await manager.RegisterUserAsync(req);

        Assert.Equal(BusinessCode.Created, result.StatusCode);
        Assert.Equal("User registered successfully.", result.Message);
        Assert.Equal(regResp, result.Data);
    }

    [Fact]
    public async Task LoginUserAsync_ReturnsBadRequest_WhenRequestIsNull()
    {
        var manager = CreateManager();
        var result = await manager.LoginUserAsync(null);

        Assert.Equal(BusinessCode.BadRequest, result.StatusCode);
        Assert.Equal("User login request cannot be null.", result.Message);
    }

    [Theory]
    [InlineData("test@test.com", "pass", BusinessCode.InternalServerError, "error")]
    public async Task LoginUserAsync_ReturnsError_WhenFindAsyncReturnsNullData(string email, string password, BusinessCode code, string message)
    {
        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>> { Data = null, StatusCode = code, Message = message });

        var manager = CreateManager();
        var req = new UserLoginRequest { Email = email, Password = password };
        var result = await manager.LoginUserAsync(req);

        Assert.Equal(code, result.StatusCode);
        Assert.Equal(message, result.Message);
    }

    [Theory]
    [InlineData("test@test.com", "pass")]
    public async Task LoginUserAsync_ReturnsNotFound_WhenUserNotFound(string email, string password)
    {
        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>> { Data = Enumerable.Empty<AuthUserDto>(), StatusCode = BusinessCode.Ok });

        var manager = CreateManager();
        var req = new UserLoginRequest { Email = email, Password = password };
        var result = await manager.LoginUserAsync(req);

        Assert.Equal(BusinessCode.NotFound, result.StatusCode);
        Assert.Equal("User not found.", result.Message);
    }

    [Theory]
    [InlineData("test@test.com", "wrongpass", "otherpass")]
    public async Task LoginUserAsync_ReturnsUnauthorized_WhenPasswordInvalid(string email, string inputPassword, string storedPassword)
    {
        var userDto = new AuthUserDto
        {
            Id = Guid.NewGuid(),
            Email = email,
            HashedPassword = new PasswordHasher<object>().HashPassword(null, storedPassword),
            Role = UserRole.User
        };
        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>> { Data = new[] { userDto }, StatusCode = BusinessCode.Ok });

        var manager = CreateManager();
        var req = new UserLoginRequest { Email = email, Password = inputPassword };
        var result = await manager.LoginUserAsync(req);

        Assert.Equal(BusinessCode.Unauthorized, result.StatusCode);
        Assert.Equal("Invalid password.", result.Message);
    }

    [Theory]
    [InlineData("test@test.com", "pass", BusinessCode.InternalServerError, "token error")]
    public async Task LoginUserAsync_ReturnsError_WhenTokenGenerationFails(string email, string password, BusinessCode code, string message)
    {
        var userDto = new AuthUserDto
        {
            Id = Guid.NewGuid(),
            Email = email,
            HashedPassword = new PasswordHasher<object>().HashPassword(null, password),
            Role = UserRole.User
        };
        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>> { Data = new[] { userDto }, StatusCode = BusinessCode.Ok });
        _userClaimsMapper.Setup(m => m.Map(It.IsAny<AuthUserDto>()))
            .Returns(new UserClaims { UserId = userDto.Id, Username = "test", Role = UserRole.User });
        _tokenManager.Setup(t => t.GenerateTokenAsync(It.IsAny<UserClaims>()))
            .ReturnsAsync(new BusinessResponse<TokenResponse> { Data = null, StatusCode = code, Message = message });

        var manager = CreateManager();
        var req = new UserLoginRequest { Email = email, Password = password };
        var result = await manager.LoginUserAsync(req);

        Assert.Equal(code, result.StatusCode);
        Assert.Equal(message, result.Message);
    }

    [Theory]
    [InlineData("test@test.com", "pass", "token created")]
    public async Task LoginUserAsync_ReturnsCreated_WhenOk(string email, string password, string tokenMessage)
    {
        var userDto = new AuthUserDto
        {
            Id = Guid.NewGuid(),
            Email = email,
            HashedPassword = new PasswordHasher<object>().HashPassword(null, password),
            Role = UserRole.User
        };
        var tokenResp = new TokenResponse { Token = "token", TokenExpiration = DateTime.UtcNow.AddHours(1) };

        _authUserService.Setup(s => s.FindAsync(It.IsAny<Expression<Func<AuthUser, bool>>>()))
            .ReturnsAsync(new BusinessResponse<IEnumerable<AuthUserDto>> { Data = new[] { userDto }, StatusCode = BusinessCode.Ok });
        _userClaimsMapper.Setup(m => m.Map(It.IsAny<AuthUserDto>()))
            .Returns(new UserClaims { UserId = userDto.Id, Username = "test", Role = UserRole.User });
        _tokenManager.Setup(t => t.GenerateTokenAsync(It.IsAny<UserClaims>()))
            .ReturnsAsync(new BusinessResponse<TokenResponse> { Data = tokenResp, StatusCode = BusinessCode.Created, Message = tokenMessage });

        var manager = CreateManager();
        var req = new UserLoginRequest { Email = email, Password = password };
        var result = await manager.LoginUserAsync(req);

        Assert.Equal(BusinessCode.Created, result.StatusCode);
        Assert.Equal(tokenMessage, result.Message);
        Assert.Equal(tokenResp, result.Data);
    }

    [Theory]
    [InlineData("sometoken", BusinessCode.Ok, "revoked")]
    public async Task LogoutUserAsync_DelegatesToTokenManager(string token, BusinessCode code, string message)
    {
        var expected = new BusinessResponse<TokenResponse> { StatusCode = code, Message = message };
        _tokenManager.Setup(t => t.RevokeTokenAsync(token)).ReturnsAsync(expected);

        var manager = CreateManager();
        var result = await manager.LogoutUserAsync(token);

        Assert.Equal(expected, result);
    }
}
