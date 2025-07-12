using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;

namespace MyDiet.Core.Business.Validators
{
    internal class CreateAuthorizationValidator : BaseAuthorizationValidator<CreateDietRequest, DietDto, int, Guid>
    {
        public CreateAuthorizationValidator(ResponseMessage message) : base(message)
        {
        }
    }
}
