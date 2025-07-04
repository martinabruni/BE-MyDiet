using BaseUtility;

namespace MyDiet.Auth.Domain.Options
{
    public class KeyPairMessageOption : ResponseMessageOption
    {
        public string KeyAlreadySet { get; set; } = "Key is already set.";
    }
}
