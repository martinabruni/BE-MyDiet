namespace MyDiet.Auth.Domain.Models
{
    public class JsonWebKeySetDto
    {
        public required List<JsonWebKeyDto> Keys { get; set; }
    }
}