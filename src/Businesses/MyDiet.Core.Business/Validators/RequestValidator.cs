using BaseUtility;
using MyDiet.Core.Domain.Validation;

namespace MyDiet.Core.Business.Validators
{
    internal class RequestValidator<TRequest, TData, TContext> : BaseValidationHandler<TRequest, TData, TContext>
        where TData : class
        where TContext : class, IContext<TData>
    {
        private readonly ResponseMessage _message;

        public RequestValidator(ResponseMessage message)
        {
            _message = message;
        }

        protected override Task<BusinessResponse<TData>> ValidateAsync(TRequest request, ContextProvider<TContext> validation)
        {
            if (request is null)
            {
                return Task.FromResult(BusinessResponse<TData>.BadRequest(_message.InvalidRequest));
            }

            return Task.FromResult(BusinessResponse<TData>.Ok(validation.Context.Data, $"{nameof(RequestValidator<TRequest, TData, TContext>)} passed"));
        }
    }
}
