using BaseUtility;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validators
{
    internal abstract class BaseAuthorizationValidator<TRequest, TData, TKey, TOwnerKey> : BaseValidationHandler<TRequest, TData, CoreValidationContext<TData, TKey>>
        where TData : class, IEntity<TKey>, IAuthorizedEntity<TOwnerKey>
        where TKey : notnull
        where TOwnerKey : notnull
    {
        private readonly ResponseMessage _message;

        public BaseAuthorizationValidator(ResponseMessage message)
        {
            _message = message;
        }

        protected override Task<BusinessResponse<TData>> ValidateAsync(TRequest request, ValidationContext<CoreValidationContext<TData, TKey>> validation)
        {
            if (validation.Context.Data is null)
            {
                return Task.FromResult(BusinessResponse<TData>.BadRequest(_message.EntityNotFound));
            }
            if (validation.Context.Data.UserId.Equals(validation.Context.UserId))
            {
                return Task.FromResult(BusinessResponse<TData>.Unauthorize(_message.InvalidRequest));
            }
            return Task.FromResult(BusinessResponse<TData>.Ok(validation.Context.Data, $"{nameof(CreateAuthorizationValidator)} passed"));
        }
    }
}
