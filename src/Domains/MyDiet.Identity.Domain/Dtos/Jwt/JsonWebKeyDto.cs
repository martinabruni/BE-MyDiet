namespace MyDiet.Identity.Domain.Dtos.Jwt
{
    public class JsonWebKeyDto
    {
        public required string Kty { get; set; }  // Key type (e.g. RSA)
        public required string Use { get; set; }  // Public key use (e.g. sig)
        public required string Kid { get; set; }  // Key ID
        public required string Alg { get; set; }  // Algorithm (e.g. RS256)
        public required string N { get; set; }    // Modulus
        public required string E { get; set; }    // Exponent
    }
}
