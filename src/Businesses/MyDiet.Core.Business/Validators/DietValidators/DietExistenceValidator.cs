using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietExistenceValidator : BaseExistenceValidator<DietDto, DietDto, Diet, int>
    {
        public DietExistenceValidator(IService<DietDto, Diet, int> service, ResponseMessage message, bool errorOnExistingEntity, bool retrieveEntity = false, bool overrideContextData = false) : base(service, message, errorOnExistingEntity, retrieveEntity, overrideContextData)
        {
        }

        protected override Expression<Func<Diet, bool>> GetFilterExpression(DietDto request)
        {
            return diet => diet.Id == request.Id;
        }
    }
}
