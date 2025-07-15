using BaseUtility;
using MyDiet.Core.Domain.Dtos.Plan;

namespace MyDiet.Core.Business.Validators.PlanValidators
{
    internal class PlanOwnershipValidator : BaseAuthorizationValidator<PlanDto, PlanDto, int, Guid>
    {
        public PlanOwnershipValidator(ResponseMessage message, bool authorizeFromOldData) : base(message, authorizeFromOldData)
        {
        }
    }
}
