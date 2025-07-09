using BaseUtility;

namespace MyDiet.Core.Domain.Options
{
    public class DietMessageOption : ResponseMessage
    {
        public string DietAlreadyExists { get; set; } = "Diet already exists.";
    }
}
