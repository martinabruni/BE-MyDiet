using BaseUtility;
using Moq;
using MyDiet.Core.Business.Managers;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Dtos.Requests;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Shared.Test.UnitTests;

public class BaseManagerTests
{
    // Test class that extends BaseManager for testing purposes
    internal class TestManager : BaseManager<TestDto, CreateTestRequest, int>
    {
        public TestManager(IService<CoreUserDto, CoreUser, Guid> userService) : base(userService)
        {
        }

        // Expose protected methods for testing
        public new Guid? ValidateUserClaim(Claim? claim) => base.ValidateUserClaim(claim);
        public new CreateTestRequest? ValidateRequest(CreateTestRequest request) => base.ValidateRequest(request);
        public new async Task<BusinessResponse<TestDto>> ValidateUserExistsAsync(Guid userId, string notFoundMessage) => 
            await base.ValidateUserExistsAsync(userId, notFoundMessage);
        public new BusinessResponse<TestDto> ValidateOwnership(Guid userId, Guid? entityUserId, string unauthorizedMessage) =>
            base.ValidateOwnership(userId, entityUserId, unauthorizedMessage);
        public new void ApplyCreationTimestamps<TEntity>(TEntity entity) where TEntity : IAuditable =>
            base.ApplyCreationTimestamps(entity);
        public new void ApplyUpdateTimestamps<TEntity>(TEntity entity, DateTime originalCreatedAt) where TEntity : IAuditable =>
            base.ApplyUpdateTimestamps(entity, originalCreatedAt);
    }

    // Test DTOs and requests
    internal class TestDto : IAuditable
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal class CreateTestRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    private readonly Mock<IService<CoreUserDto, CoreUser, Guid>> _userService = new();
    private readonly TestManager _manager;

    public BaseManagerTests()
    {
        _manager = new TestManager(_userService.Object);
    }

    [Fact]
    public void ValidateUserClaim_ReturnsNull_WhenClaimIsNull()
    {
        // Arrange
        Claim? claim = null;

        // Act
        var result = _manager.ValidateUserClaim(claim);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateUserClaim_ReturnsNull_WhenClaimValueIsNotValidGuid()
    {
        // Arrange
        var claim = new Claim("userId", "invalid-guid");

        // Act
        var result = _manager.ValidateUserClaim(claim);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateUserClaim_ReturnsUserId_WhenClaimValueIsValidGuid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claim = new Claim("userId", userId.ToString());

        // Act
        var result = _manager.ValidateUserClaim(claim);

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public void ValidateRequest_ReturnsNull_WhenRequestIsNull()
    {
        // Arrange
        CreateTestRequest? request = null;

        // Act
        var result = _manager.ValidateRequest(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateRequest_ReturnsRequest_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateTestRequest { Name = "Test" };

        // Act
        var result = _manager.ValidateRequest(request);

        // Assert
        Assert.Equal(request, result);
    }

    [Fact]
    public async Task ValidateUserExistsAsync_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notFoundMessage = "User not found";
        _userService.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(BusinessResponse<CoreUserDto>.NotFound(notFoundMessage));

        // Act
        var result = await _manager.ValidateUserExistsAsync(userId, notFoundMessage);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(BusinessCode.NotFound, result.StatusCode);
        Assert.Equal(notFoundMessage, result.Message);
    }

    [Fact]
    public async Task ValidateUserExistsAsync_ReturnsNull_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new CoreUserDto { Id = userId };
        var notFoundMessage = "User not found";
        _userService.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(BusinessResponse<CoreUserDto>.Ok(user, "Success"));

        // Act
        var result = await _manager.ValidateUserExistsAsync(userId, notFoundMessage);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateOwnership_ReturnsUnauthorize_WhenUserDoesNotOwnEntity()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var entityUserId = Guid.NewGuid();
        var unauthorizedMessage = "Unauthorized";

        // Act
        var result = _manager.ValidateOwnership(userId, entityUserId, unauthorizedMessage);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(BusinessCode.Unauthorize, result.StatusCode);
        Assert.Equal(unauthorizedMessage, result.Message);
    }

    [Fact]
    public void ValidateOwnership_ReturnsNull_WhenUserOwnsEntity()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var unauthorizedMessage = "Unauthorized";

        // Act
        var result = _manager.ValidateOwnership(userId, userId, unauthorizedMessage);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ApplyCreationTimestamps_SetsCreatedAtAndUpdatedAtToSameValue()
    {
        // Arrange
        var testDto = new TestDto();
        var beforeTest = DateTime.UtcNow;

        // Act
        _manager.ApplyCreationTimestamps(testDto);
        var afterTest = DateTime.UtcNow;

        // Assert
        Assert.True(testDto.CreatedAt >= beforeTest && testDto.CreatedAt <= afterTest);
        Assert.Equal(testDto.CreatedAt, testDto.UpdatedAt);
    }

    [Fact]
    public void ApplyUpdateTimestamps_PreservesCreatedAtAndUpdatesUpdatedAt()
    {
        // Arrange
        var testDto = new TestDto();
        var originalCreatedAt = DateTime.UtcNow.AddDays(-1);
        var beforeTest = DateTime.UtcNow;

        // Act
        _manager.ApplyUpdateTimestamps(testDto, originalCreatedAt);
        var afterTest = DateTime.UtcNow;

        // Assert
        Assert.Equal(originalCreatedAt, testDto.CreatedAt);
        Assert.True(testDto.UpdatedAt >= beforeTest && testDto.UpdatedAt <= afterTest);
        Assert.True(testDto.UpdatedAt > testDto.CreatedAt);
    }
}