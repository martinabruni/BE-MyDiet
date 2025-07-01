namespace MyDiet.Auth.Domain.Options
{
    public class JsonWebKeyOption
    {
        public required string Kty { get; set; }
        public required string Use { get; set; }
        public required string Alg { get; set; }
    }
}
