using BaseUtility;

namespace MyDiet.Auth.Domain.Options
{
    public class KeyPairMessageOption : ResponseMessage
    {
        public string KeyAlreadySet { get; set; } = "Key is already set.";
    }
}
