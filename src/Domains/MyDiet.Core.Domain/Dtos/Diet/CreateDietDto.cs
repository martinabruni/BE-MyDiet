namespace MyDiet.Core.Domain.Dtos.Diet
{
    public class CreateDietDto : CreateDietRequest
    {
        public Guid UserId { get; set; }
    }
}
