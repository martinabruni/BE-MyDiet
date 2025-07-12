using BaseUtility;
using MyDiet.Core.Business.Validators.DietValidators;
using MyDiet.Core.Domain.Responses;
using MyDiet.Core.Domain.Validation;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators
{
    internal abstract class BaseExistenceValidator<TRequest, TData, TDatabase, TKey> : BaseValidationHandler<TRequest, TData, CoreValidationContext<TData, TKey>>
        where TData : class, IEntity<TKey>
        where TDatabase : class, IEntity<TKey>
        where TKey : notnull
    {
        private readonly IService<TData, TDatabase, TKey> _dietService;
        private readonly DietMessage _dietMessage;
        private bool _errorOnExistingDiet;

        public BaseExistenceValidator(IService<TData, TDatabase, TKey> dietService, DietMessage dietMessage, bool errorOnExistingDiet)
        {
            _dietService = dietService;
            _dietMessage = dietMessage;
            _errorOnExistingDiet = errorOnExistingDiet;
        }

        protected abstract Expression<Func<TDatabase, bool>> GetFilterExpression(TRequest request);

        protected override async Task<BusinessResponse<TData>> ValidateAsync(TRequest request, ValidationContext<CoreValidationContext<TData, TKey>> validation)
        {
            var userId = validation.Context.UserId;

            var existingDietRes = await _dietService.FindAsync(GetFilterExpression(request));

            if (existingDietRes.Data is null)
            {
                return BusinessResponse<TData>.InternalServerError(_dietMessage.ErrorRetrievingEntities);
            }
            if (existingDietRes.Data.ToList().Count != 0 && _errorOnExistingDiet)
            {
                return BusinessResponse<TData>.BadRequest(_dietMessage.DietAlreadyExists);
            }
            if (existingDietRes.Data.ToList().Count == 0 && !_errorOnExistingDiet)
            {
                return BusinessResponse<TData>.NotFound(_dietMessage.EntityNotFound);
            }

            validation.Context.Data = existingDietRes.Data.FirstOrDefault();

            return BusinessResponse<TData>.Ok($"{nameof(DietNameUniquenessValidator)} passed");
        }
    }
}
