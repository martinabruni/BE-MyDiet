using BaseUtility;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Domain.Managers;
using MyDiet.Core.Infrastructure.Models;
using System.Security.Claims;

namespace MyDiet.Core.Business.Managers
{
    internal abstract class BaseManager<TResponse, TRequest, TKey> : IValidationManager<TRequest>
        where TResponse : class
        where TRequest : class
        where TKey : notnull
    {
        protected readonly IService<CoreUserDto, CoreUser, Guid> _userService;

        protected BaseManager(IService<CoreUserDto, CoreUser, Guid> userService)
        {
            _userService = userService;
        }

        #region Basic Validation Methods

        public Guid? ValidateUserClaim(Claim? claim)
        {
            if (claim is null || !Guid.TryParse(claim.Value, out var userId))
            {
                return null;
            }
            return userId;
        }

        public TRequest? ValidateRequest(TRequest request)
        {
            if (request is null)
            {
                return null;
            }
            return request;
        }

        public virtual Guid? ValidateAndGetUserId(TRequest request, Claim? claim)
        {
            var validRequest = ValidateRequest(request);
            if (validRequest is null)
            {
                return null;
            }
            var userId = ValidateUserClaim(claim);
            if (userId is null)
            {
                return null;
            }
            return userId;
        }

        #endregion

        #region Template Methods for Common Validation Flows

        /// <summary>
        /// Template method to validate user existence
        /// </summary>
        protected virtual async Task<BusinessResponse<TResponse>> ValidateUserExistsAsync(Guid userId, string notFoundMessage)
        {
            var userRes = await _userService.GetByIdAsync(userId);
            if (userRes.Data is null)
            {
                return BusinessResponse<TResponse>.NotFound(notFoundMessage);
            }
            return null; // User exists, validation passed
        }

        /// <summary>
        /// Template method for entity existence validation
        /// </summary>
        protected virtual async Task<BusinessResponse<TEntity>> ValidateEntityExistsAsync<TEntity>(
            IService<TEntity, object, TKey> service, 
            TKey id, 
            string notFoundMessage) where TEntity : class
        {
            var entityRes = await service.GetByIdAsync(id);
            if (entityRes.Data is null)
            {
                return BusinessResponse<TEntity>.NotFound(notFoundMessage);
            }
            return entityRes;
        }

        /// <summary>
        /// Template method for ownership validation
        /// </summary>
        protected virtual BusinessResponse<TResponse> ValidateOwnership(Guid userId, Guid? entityUserId, string unauthorizedMessage)
        {
            if (entityUserId != userId)
            {
                return BusinessResponse<TResponse>.Unauthorize(unauthorizedMessage);
            }
            return null; // Ownership validated
        }

        /// <summary>
        /// Template method to apply timestamps for creation
        /// </summary>
        protected virtual void ApplyCreationTimestamps<TEntity>(TEntity entity) where TEntity : IAuditable
        {
            var now = DateTime.UtcNow;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
        }

        /// <summary>
        /// Template method to apply timestamps for updates
        /// </summary>
        protected virtual void ApplyUpdateTimestamps<TEntity>(TEntity entity, DateTime originalCreatedAt) where TEntity : IAuditable
        {
            entity.CreatedAt = originalCreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Template Methods for CRUD Operations

        /// <summary>
        /// Template method for validating user and request for operations requiring both
        /// </summary>
        protected virtual async Task<(Guid userId, BusinessResponse<TResponse> error)> ValidateUserAndRequestAsync(
            TRequest request, 
            Claim? userIdClaim, 
            string invalidRequestMessage,
            string userNotFoundMessage)
        {
            var userId = ValidateAndGetUserId(request, userIdClaim);
            if (userId is null)
            {
                return (Guid.Empty, BusinessResponse<TResponse>.BadRequest(invalidRequestMessage));
            }

            var userValidation = await ValidateUserExistsAsync(userId.Value, userNotFoundMessage);
            if (userValidation != null)
            {
                return (Guid.Empty, userValidation);
            }

            return (userId.Value, null);
        }

        /// <summary>
        /// Template method for validating user for operations requiring only user claim
        /// </summary>
        protected virtual async Task<(Guid userId, BusinessResponse<TResponse> error)> ValidateUserAsync(
            Claim? userIdClaim,
            string invalidRequestMessage,
            string userNotFoundMessage)
        {
            var userId = ValidateUserClaim(userIdClaim);
            if (userId is null)
            {
                return (Guid.Empty, BusinessResponse<TResponse>.BadRequest(invalidRequestMessage));
            }

            var userValidation = await ValidateUserExistsAsync(userId.Value, userNotFoundMessage);
            if (userValidation != null)
            {
                return (Guid.Empty, userValidation);
            }

            return (userId.Value, null);
        }

        #endregion

        #region Specialized Template Methods for Complex Ownership

        /// <summary>
        /// Template method for validating indirect ownership through parent entity (e.g., Plan -> Diet -> User)
        /// </summary>
        protected virtual async Task<BusinessResponse<TResponse>> ValidateIndirectOwnershipAsync<TParentDto>(
            IService<TParentDto, object, TKey> parentService,
            TKey parentId,
            Guid userId,
            Func<TParentDto, Guid?> getUserIdFromParent,
            string parentNotFoundMessage,
            string unauthorizedMessage) where TParentDto : class
        {
            var parentRes = await ValidateEntityExistsAsync(parentService, parentId, parentNotFoundMessage);
            if (parentRes.Data is null)
            {
                return BusinessResponse<TResponse>.NotFound(parentNotFoundMessage);
            }

            var parentUserId = getUserIdFromParent(parentRes.Data);
            var ownershipError = ValidateOwnership(userId, parentUserId, unauthorizedMessage);
            if (ownershipError != null)
            {
                return ownershipError;
            }

            return null; // Validation passed
        }

        #endregion
    }
}
