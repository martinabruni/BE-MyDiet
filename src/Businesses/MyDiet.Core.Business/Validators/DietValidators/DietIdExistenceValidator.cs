using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietIdExistenceValidator : BaseAuthorizedExistenceValidator<int, DietDto, Diet, int>
    {
        public DietIdExistenceValidator(IService<DietDto, Diet, int> service, ResponseMessage message, bool errorOnExistingEntity, bool retrieveOldEntity = false, bool overrideContextData = false) : base(service, message, errorOnExistingEntity, retrieveOldEntity, overrideContextData)
        {
        }

        protected override Expression<Func<Diet, bool>> GetFilterExpression(int request)
        {
            return d => d.Id == request;
        }
    }
}
