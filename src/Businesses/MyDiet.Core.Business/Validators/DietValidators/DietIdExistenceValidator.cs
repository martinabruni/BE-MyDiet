using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Responses;
using MyDiet.Core.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietIdExistenceValidator : BaseExistenceValidator<int, DietDto, Diet, int>
    {
        public DietIdExistenceValidator(IService<DietDto, Diet, int> dietService, DietMessage dietMessage, bool errorOnExistingDiet) : base(dietService, dietMessage, errorOnExistingDiet)
        {
        }

        protected override Expression<Func<Diet, bool>> GetFilterExpression(int request)
        {
            return d => d.Id == request;
        }
    }
}
