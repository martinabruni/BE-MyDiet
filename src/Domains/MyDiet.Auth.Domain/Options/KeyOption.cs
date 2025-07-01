namespace MyDiet.Auth.Domain.Options
{
    public class KeyOption
    {
        public required string PrivateKeyName { get; set; }
        public int KeySize { get; set; } = 2048;
    }
}
