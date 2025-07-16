using BaseUtility;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators.PlanValidators
{
    internal class PlanNameUniquenessValidator : BaseAuthorizedExistenceValidator<CreatePlanRequest, PlanDto, Plan, int>
    {
        public PlanNameUniquenessValidator(IService<PlanDto, Plan, int> service, ResponseMessage message, bool errorOnExistingEntity, bool retrieveOldEntity = false, bool overrideContextData = false) : base(service, message, errorOnExistingEntity, retrieveOldEntity, overrideContextData)
        {
        }

        protected override Expression<Func<Plan, bool>> GetFilterExpression(CreatePlanRequest request)
        {
            return plan => plan.Name == request.Name;
        }
    }
}
