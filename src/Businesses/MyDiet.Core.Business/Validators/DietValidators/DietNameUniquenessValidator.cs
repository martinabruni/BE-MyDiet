using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietNameUniquenessValidator : BaseAuthorizedExistenceValidator<CreateDietRequest, DietDto, Diet, int>
    {
        public DietNameUniquenessValidator(IService<DietDto, Diet, int> service, ResponseMessage message, bool errorOnExistingEntity, bool retrieveOldEntity = false) : base(service, message, errorOnExistingEntity, retrieveOldEntity)
        {
        }

        protected override Expression<Func<Diet, bool>> GetFilterExpression(CreateDietRequest request)
        {
            return d => d.Name == request.Name;
        }
    }
}
