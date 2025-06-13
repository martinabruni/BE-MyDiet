namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IKeyProvider
    {
        Task<string> GetPublicKeyAsync();
    }
}
