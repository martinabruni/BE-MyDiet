using BaseUtility;

namespace MyDiet.Core.Business.Validation
{
    internal class RequestValidationHandler<TRequest, TData, TContext> : BaseValidationHandler<TRequest, TData, TContext>
        where TRequest : class
        where TData : class
        where TContext : class
    {
        private readonly ResponseMessage _message;

        public RequestValidationHandler(ResponseMessage message)
        {
            _message = message;
        }

        protected override Task<BusinessResponse<TData>> ValidateAsync(TRequest request, ValidationContext<TContext> context)
        {
            if(request is null)
            {
                return Task.FromResult(BusinessResponse<TData>.BadRequest(_message.InvalidRequest));
            }

            return Task.FromResult(BusinessResponse<TData>.Ok($"{nameof(RequestValidationHandler<TRequest, TData, TContext>)} passed"));
        }
    }
}
