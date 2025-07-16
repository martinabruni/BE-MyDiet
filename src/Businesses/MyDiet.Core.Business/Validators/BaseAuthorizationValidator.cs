using BaseUtility;
using MyDiet.Core.Business.Validators.DietValidators;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validators
{
    internal abstract class BaseAuthorizationValidator<TRequest, TData, TKey, TOwnerKey> : BaseValidationHandler<TRequest, TData, CoreValidationContext<TData, TKey>>
        where TData : class, IEntity<TKey>, IAuthorizedEntity<TOwnerKey>
    {
        private readonly ResponseMessage _message;
        private readonly bool _authorizeFromOldData;

        public BaseAuthorizationValidator(ResponseMessage message, bool authorizeFromOldData)
        {
            _message = message;
            _authorizeFromOldData = authorizeFromOldData;
        }

        protected override Task<BusinessResponse<TData>> ValidateAsync(TRequest request, ContextProvider<CoreValidationContext<TData, TKey>> validation)
        {
            if (validation.Context.Data is null)
            {
                return Task.FromResult(BusinessResponse<TData>.InternalServerError(_message.ErrorRetrievingEntity));
            }
            TOwnerKey authorizedUser;
            if (_authorizeFromOldData)
            {
                if (validation.Context.OldData is null)
                {
                    return Task.FromResult(BusinessResponse<TData>.InternalServerError(_message.ErrorRetrievingEntity));
                }
                authorizedUser = validation.Context.OldData.UserId;
            }
            else
            {
                if (validation.Context.Data.UserId is null)
                {
                    return Task.FromResult(BusinessResponse<TData>.InternalServerError(_message.ErrorRetrievingEntity));
                }
                authorizedUser = validation.Context.Data.UserId;
            }
            if (!authorizedUser.Equals(validation.Context.UserId))
            {
                return Task.FromResult(BusinessResponse<TData>.Unauthorize(_message.InvalidRequest));
            }
            return Task.FromResult(BusinessResponse<TData>.Ok(validation.Context.Data, $"{nameof(DietOwnershipValidator)} passed"));
        }
    }
}
