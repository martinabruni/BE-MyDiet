using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;

namespace MyDiet.Core.Business.Validators.DietValidators
{
    internal class DietIdOwnershipValidator : BaseAuthorizationValidator<int, DietDto, int, Guid>
    {
        public DietIdOwnershipValidator(ResponseMessage message) : base(message)
        {
        }
    }
}
