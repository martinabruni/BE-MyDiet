using BaseUtility;
using MyDiet.Core.Domain.Dtos.Plan;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Services
{
    internal class PlanService : BaseService<PlanDto, Plan, int>, IService<PlanDto, Plan, int>
    {
        public PlanService(IRepository<Plan, int> repository, IMapper<Plan, PlanDto> databaseToDtoMapper, IMapper<PlanDto, Plan> dtoToDatabaseMapper, ResponseMessage messages) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper, messages)
        {
        }
    }
}
