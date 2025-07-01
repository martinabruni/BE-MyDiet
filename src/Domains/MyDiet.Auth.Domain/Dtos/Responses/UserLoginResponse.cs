namespace MyDiet.Auth.Domain.Dtos.Responses
{

    //TODO: da rimuovere?
    public class UserLoginResponse
    {
        public required string Token { get; set; }
        public required DateTime TokenExpiration { get; set; }
    }
}
