namespace MyDiet.Identity.Domain.Dtos.Jwt
{
    public class JsonWebKeySetDto
    {
        public required List<JsonWebKeyDto> Keys { get; set; }
    }
}
