using BaseUtility;
using MyDiet.Core.Business.Validators.DietValidators;
using MyDiet.Core.Domain.Validation;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators
{
    internal abstract class BaseAuthorizedExistenceValidator<TRequest, TData, TDatabase, TKey> : BaseValidationHandler<TRequest, TData, CoreValidationContext<TData, TKey>>
        where TData : class, IEntity<TKey>
        where TDatabase : class, IEntity<TKey>, IAuthorizedEntity<Guid?>
    {
        private readonly IService<TData, TDatabase, TKey> _service;
        private readonly ResponseMessage _message;
        private bool _errorOnExistingEntity;
        private bool _retrieveOldEntity;
        private bool _overrideContextData;

        public BaseAuthorizedExistenceValidator(IService<TData, TDatabase, TKey> service, ResponseMessage message, bool errorOnExistingEntity, bool retrieveOldEntity = false, bool overrideContextData = false)
        {
            _service = service;
            _message = message;
            _errorOnExistingEntity = errorOnExistingEntity;
            _retrieveOldEntity = retrieveOldEntity;
            _overrideContextData = overrideContextData;
        }

        protected abstract Expression<Func<TDatabase, bool>> GetFilterExpression(TRequest request);

        protected override async Task<BusinessResponse<TData>> ValidateAsync(TRequest request, ContextProvider<CoreValidationContext<TData, TKey>> validation)
        {
            var userId = validation.Context.UserId;

            Expression<Func<TDatabase, bool>> baseFilter = GetFilterExpression(request);
            Expression<Func<TDatabase, bool>> activeFilter = p => p.UserId == userId;

            var parameter = baseFilter.Parameters[0];
            var combined = Expression.Lambda<Func<TDatabase, bool>>(
                Expression.AndAlso(
                    baseFilter.Body,
                    Expression.Invoke(activeFilter, parameter)
                ),
                parameter
            );

            var existingEntityRes = await _service.FindAsync(combined);
            var existingEntities = existingEntityRes.Data;

            if (existingEntities is null)
            {
                return BusinessResponse<TData>.InternalServerError(_message.ErrorRetrievingEntities);
            }

            var entityCount = existingEntities.ToList().Count();

            if (entityCount != 0 && _errorOnExistingEntity)
            {
                return BusinessResponse<TData>.BadRequest(_message.EntityAlreadyExists);
            }
            if (entityCount == 0 && !_errorOnExistingEntity)
            {
                return BusinessResponse<TData>.NotFound(_message.EntityNotFound);
            }

            var existingEntity = existingEntities.FirstOrDefault();

            if (_retrieveOldEntity)
                validation.Context.OldData = existingEntity;
            if(_overrideContextData)
                validation.Context.Data = existingEntity;

            return BusinessResponse<TData>.Ok(validation.Context.Data, $"{nameof(DietNameUniquenessValidator)} passed");
        }
    }
}
