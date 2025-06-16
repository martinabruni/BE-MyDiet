namespace MyDiet.Identity.Domain.Dtos
{
    public class JwkDto
    {
        public string Kty { get; set; } = "";  // Key type (e.g. RSA)
        public string Use { get; set; } = "";  // Public key use (e.g. sig)
        public string Kid { get; set; } = "";  // Key ID
        public string Alg { get; set; } = "";  // Algorithm (e.g. RS256)
        public string N { get; set; } = "";    // Modulus
        public string E { get; set; } = "";    // Exponent
    }
}
