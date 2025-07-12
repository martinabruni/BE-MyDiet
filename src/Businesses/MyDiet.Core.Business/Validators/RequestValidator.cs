using BaseUtility;

namespace MyDiet.Core.Business.Validators
{
    internal class RequestValidator<TRequest, TData, TContext> : BaseValidationHandler<TRequest, TData, TContext>
        where TData : class
        where TContext : class
    {
        private readonly ResponseMessage _message;

        public RequestValidator(ResponseMessage message)
        {
            _message = message;
        }

        protected override Task<BusinessResponse<TData>> ValidateAsync(TRequest request, ValidationContext<TContext> context)
        {
            if(request is null)
            {
                return Task.FromResult(BusinessResponse<TData>.BadRequest(_message.InvalidRequest));
            }

            return Task.FromResult(BusinessResponse<TData>.Ok($"{nameof(RequestValidator<TRequest, TData, TContext>)} passed"));
        }
    }
}
