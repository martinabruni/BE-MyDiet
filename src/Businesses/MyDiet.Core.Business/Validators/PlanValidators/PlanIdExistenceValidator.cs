using BaseUtility;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;
using System.Linq.Expressions;

namespace MyDiet.Core.Business.Validators.PlanValidators
{
    internal class PlanIdExistenceValidator : BaseAuthorizedExistenceValidator<int, PlanDto, Plan, int>
    {
        public PlanIdExistenceValidator(IService<PlanDto, Plan, int> service, ResponseMessage message, bool errorOnExistingEntity, bool retrieveOldEntity = false, bool overrideContextData = false) : base(service, message, errorOnExistingEntity, retrieveOldEntity, overrideContextData)
        {
        }

        protected override Expression<Func<Plan, bool>> GetFilterExpression(int request)
        {
            return plan => plan.Id == request;
        }
    }
}
