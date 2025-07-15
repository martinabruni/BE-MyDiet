using BaseUtility;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators.PlanValidators
{
    internal class PlanExistenceValidator : BaseExistenceValidator<PlanDto, PlanDto, Plan, int>
    {
        public PlanExistenceValidator(IService<PlanDto, Plan, int> service, ResponseMessage message, bool errorOnExistingEntity, bool retrieveEntity = false, bool overrideContextData = false) : base(service, message, errorOnExistingEntity, retrieveEntity, overrideContextData)
        {
        }

        protected override Expression<Func<Plan, bool>> GetFilterExpression(PlanDto request)
        {
            return plan => plan.Id == request.Id;
        }
    }
}
