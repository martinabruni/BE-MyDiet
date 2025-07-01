using MyDiet.Auth.Domain.Enums;

namespace MyDiet.Auth.Domain.Dtos.Claims
{
    public class UserClaims
    {
        public Guid UserId { get; set; }
        public UserRole Role { get; set; }
    }
}
