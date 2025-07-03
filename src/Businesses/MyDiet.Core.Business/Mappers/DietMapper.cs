using BaseUtility;
using MyDiet.Shared.Domain.Dtos;
using MyDiet.Shared.Infrastructure.Models;

namespace MyDiet.Shared.Business.Mappers
{
    internal class DietMapper : IMapper<Diet, DietDto>, IMapper<DietDto, Diet>
    {
        public Diet Map(DietDto input)
        {
            return new Diet
            {
                Id = input.Id,
                UserId = input.UserId,
                Name = input.Name,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt
            };
        }

        public DietDto Map(Diet input)
        {
            return new DietDto
            {
                Id = input.Id,
                UserId = input.UserId,
                Name = input.Name,
                CreatedAt = input.CreatedAt,
                UpdatedAt = input.UpdatedAt
            };
        }
    }
}
