using BaseUtility;
using MyDiet.Core.Domain.Dtos.CoreUser;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Validators
{
    internal class UserAuthenticationValidator<TRequest, TData, TContext, TKey> : BaseValidationHandler<TRequest, TData, CoreValidationContext<TData, TKey>>
        where TData : class
        where TContext : class
    {
        private readonly IService<CoreUserDto, CoreUser, Guid> _userService;
        private readonly ResponseMessage _message;

        public UserAuthenticationValidator(IService<CoreUserDto, CoreUser, Guid> userService, ResponseMessage message)
        {
            _userService = userService;
            _message = message;
        }

        protected override async Task<BusinessResponse<TData>> ValidateAsync(TRequest request, ContextProvider<CoreValidationContext<TData, TKey>> validation)
        {
            if (validation.Context.UserClaim is null)
            {
                return BusinessResponse<TData>.Unauthorize(_message.InvalidRequest);
            }
            if (!Guid.TryParse(validation.Context.UserClaim.Value, out var userId))
            {
                return BusinessResponse<TData>.Unauthorize(_message.InvalidRequest);
            }

            var userRes = await _userService.GetByIdAsync(userId);
            if (userRes.Data is null)
            {
                return BusinessResponse<TData>.NotFound(_message.EntityNotFound);
            }

            validation.Context.UserId = userId;
            return BusinessResponse<TData>.Ok(validation.Context.Data, $"{nameof(UserAuthenticationValidator<TRequest, TData, TContext, TKey>)} passed");
        }
    }
}
