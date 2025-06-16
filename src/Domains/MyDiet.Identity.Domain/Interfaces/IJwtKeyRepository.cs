namespace MyDiet.Identity.Domain.Interfaces
{
    public interface IJwtKeyRepository<TData>
    {
        Task<TData?> CreatePrivateKeyAsync(TData value);
        Task<TData?> GetPublicKeyAsync();
    }
}
