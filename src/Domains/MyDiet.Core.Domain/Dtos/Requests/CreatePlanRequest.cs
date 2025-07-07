namespace MyDiet.Core.Domain.Dtos.Requests
{
    public class CreatePlanRequest
    {
        public required string Name { get; set; }
        public required int DietId { get; set; }
    }
}
