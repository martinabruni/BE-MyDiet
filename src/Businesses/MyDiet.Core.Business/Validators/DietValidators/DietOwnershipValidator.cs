using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietOwnershipValidator : BaseAuthorizationValidator<DietDto, DietDto, int, Guid>
    {
        public DietOwnershipValidator(ResponseMessage message, bool authorizeFromOldData) : base(message, authorizeFromOldData)
        {
        }
    }
}
