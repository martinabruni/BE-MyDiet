using BaseUtility;

namespace MyDiet.Auth.Domain.Options
{
    public class AuthManagerMessageOption : ResponseMessage
    {
        public string UserAlreadyExists { get; set; } = "User with this email already exists.";
        public string UserRegistrationSuccess { get; set; } = "User registered successfully.";
        public string UserRegistrationFailure { get; set; } = "User registration failed.";
        public string UserNotRegistered { get; set; } = "User is not registered";
    }
}
