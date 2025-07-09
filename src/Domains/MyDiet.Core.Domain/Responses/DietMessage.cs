using BaseUtility;

namespace MyDiet.Core.Domain.Responses
{
    public class DietMessage : ResponseMessage
    {
        public string DietAlreadyExists { get; set; } = "Diet already exists.";
    }
}
