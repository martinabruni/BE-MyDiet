using BaseUtility;
using MyDiet.Core.Domain.Dtos.Plan;

namespace MyDiet.Core.Business.Validators.PlanValidators
{
    internal class PlanIdOwnershipValidator : BaseAuthorizationValidator<int, PlanDto, int, Guid>
    {
        public PlanIdOwnershipValidator(ResponseMessage message, bool authorizeFromOldData) : base(message, authorizeFromOldData)
        {
        }
    }
}
