using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietIdExistenceValidator : BaseExistenceValidator<int, DietDto, Diet, int>
    {
        public DietIdExistenceValidator(IService<DietDto, Diet, int> service, ResponseMessage message, bool errorOnExistingEntity, bool retrieveEntity = false, bool overrideContextData = false) : base(service, message, errorOnExistingEntity, retrieveEntity, overrideContextData)
        {
        }

        protected override Expression<Func<Diet, bool>> GetFilterExpression(int request)
        {
            return d => d.Id == request;
        }
    }
}
