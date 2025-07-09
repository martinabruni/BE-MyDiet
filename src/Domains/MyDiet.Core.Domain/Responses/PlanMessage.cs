using BaseUtility;

namespace MyDiet.Core.Domain.Responses
{
    public class PlanMessage : ResponseMessage
    {
        public string PlanAlreadyExists { get; set; } = "Plan with this name already exists.";
    }
}
