using BaseUtility;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Business.Services
{
    internal class DietService : AGenericService<DietDto, Diet, int>
    {
        public DietService(IRepository<Diet, int> repository, IMapper<Diet, DietDto> databaseToDtoMapper, IMapper<DietDto, Diet> dtoToDatabaseMapper) : base(repository, databaseToDtoMapper, dtoToDatabaseMapper)
        {
        }
    }
}
