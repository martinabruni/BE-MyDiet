using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Responses;
using MyDiet.Core.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators
{
    internal class CreateDietExistenceValidator : BaseExistenceValidator<CreateDietRequest, DietDto, Diet, int>
    {
        public CreateDietExistenceValidator(IService<DietDto, Diet, int> dietService, DietMessage dietMessage, bool errorOnExistingDiet) : base(dietService, dietMessage, errorOnExistingDiet)
        {
        }

        protected override Expression<Func<Diet, bool>> GetFilterExpression(CreateDietRequest request)
        {
            return d => d.Name == request.Name;
        }
    }
}
