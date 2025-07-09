using BaseUtility;
using MyDiet.Core.Domain.Dtos.Diet;
using MyDiet.Core.Infrastructure.Models;

namespace MyDiet.Core.Business.Services
{
    internal class DietService : BaseService<DietDto, Diet, int>
    {
        public DietService(IRepository<Diet, int> repository, IMapper<Diet, DietDto> databaseToDtoMapper, IMapper<DietDto, Diet> dtoToDatabaseMapper, ResponseMessage messages) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper, messages)
        {
        }
    }
}
