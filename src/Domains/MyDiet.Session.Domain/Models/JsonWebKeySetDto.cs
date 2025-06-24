namespace MyDiet.Session.Domain.Models
{
    public class JsonWebKeySetDto
    {
        public required List<JsonWebKeyDto> Keys { get; set; }
    }
}