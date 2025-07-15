using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Domain.Mappers;
using MyDiet.Core.Domain.Validation;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Mappers
{
    internal class ValidationContextMapper : IAsyncMapper<ContextProvider<CoreValidationContext<PlanDto, int>>, ContextProvider<CoreValidationContext<DietDto, int>>>
    {
        private readonly IService<DietDto, Diet, int> _dietService;

        public ValidationContextMapper(IService<DietDto, Diet, int> dietService)
        {
            _dietService = dietService;
        }

        public async Task<ContextProvider<CoreValidationContext<DietDto, int>>> Map(ContextProvider<CoreValidationContext<PlanDto, int>> input)
        {
            DietDto? diet = null;
            if (input.Context.Data is not null)
                diet = (await _dietService.GetByIdAsync(input.Context.Data.DietId)).Data;
            return new ContextProvider<CoreValidationContext<DietDto, int>>()
            {
                Context = new CoreValidationContext<DietDto, int>()
                {
                    UserId = input.Context.UserId,
                    UserClaim = input.Context.UserClaim,
                    Data = diet is null ? null : diet
                },
            };
        }
    }
}
