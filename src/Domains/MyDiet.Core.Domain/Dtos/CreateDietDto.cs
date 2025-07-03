using MyDiet.Core.Domain.Dtos.Requests;

namespace MyDiet.Core.Domain.Dtos
{
    public class CreateDietDto : CreateDietRequest
    {
        public Guid UserId { get; set; }
    }
}
