namespace MyDiet.Identity.Domain.Options
{
    public class KeyVaultOption
    {
        public required string PrivateKeyName { get; set; }
        public int KeySize { get; set; } = 2048;
    }
}
