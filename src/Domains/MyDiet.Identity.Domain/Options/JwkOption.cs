namespace MyDiet.Identity.Domain.Options
{
    public class JwkOption
    {
        public required string Kty { get; set; }  // Key type (e.g. RSA)
        public required string Use { get; set; }  // Public key use (e.g. sig)
        public required string Alg { get; set; }  // Algorithm (e.g. RS256)
    }
}
