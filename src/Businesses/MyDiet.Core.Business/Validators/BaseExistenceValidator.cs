using BaseUtility;
using MyDiet.Core.Business.Validators.DietValidators;
using MyDiet.Core.Domain.Validation;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators
{
    internal abstract class BaseExistenceValidator<TRequest, TData, TDatabase, TKey> : BaseValidationHandler<TRequest, TData, CoreValidationContext<TData, TKey>>
        where TData : class, IEntity<TKey>
        where TDatabase : class, IEntity<TKey>
        where TKey : notnull
    {
        private readonly IService<TData, TDatabase, TKey> _service;
        private readonly ResponseMessage _message;
        private bool _errorOnExistingEntity;
        private bool _retrieveOldEntity;
        private bool _overrideContextData;

        public BaseExistenceValidator(IService<TData, TDatabase, TKey> service, ResponseMessage message, bool errorOnExistingEntity, bool retrieveEntity = false, bool overrideContextData = false)
        {
            _service = service;
            _message = message;
            _errorOnExistingEntity = errorOnExistingEntity;
            _retrieveOldEntity = retrieveEntity;
            _overrideContextData = overrideContextData;
        }

        protected abstract Expression<Func<TDatabase, bool>> GetFilterExpression(TRequest request);

        protected override async Task<BusinessResponse<TData>> ValidateAsync(TRequest request, ContextProvider<CoreValidationContext<TData, TKey>> validation)
        {
            var userId = validation.Context.UserId;
            var existingEntityRes = await _service.FindAsync(GetFilterExpression(request));
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
