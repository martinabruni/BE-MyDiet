namespace MyDiet.Auth.Domain.Dtos
{
    public class JsonWebKeySetDto
    {
        public required List<JsonWebKeyDto> Keys { get; set; }
    }
}