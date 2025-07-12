using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietCreateOwnershipValidator : BaseAuthorizationValidator<CreateDietRequest, DietDto, int, Guid>
    {
        public DietCreateOwnershipValidator(ResponseMessage message) : base(message)
        {
        }
    }
}
