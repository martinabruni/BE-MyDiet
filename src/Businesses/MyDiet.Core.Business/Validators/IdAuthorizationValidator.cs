using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;

namespace MyDiet.Core.Business.Validators
{
    internal class IdAuthorizationValidator : BaseAuthorizationValidator<int, DietDto, int, Guid>
    {
        public IdAuthorizationValidator(ResponseMessage message) : base(message)
        {
        }
    }
}
