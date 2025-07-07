using BaseUtility;
using MyDiet.Core.Domain.Dtos;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Services
{
    internal class PlanService : AGenericService<PlanDto, Plan, int>, IService<PlanDto, Plan, int>
    {
        public PlanService(IRepository<Plan, int> repository, IMapper<Plan, PlanDto> databaseToDtoMapper, IMapper<PlanDto, Plan> dtoToDatabaseMapper, ResponseMessageOption messages) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper, messages)
        {
        }
    }
}
