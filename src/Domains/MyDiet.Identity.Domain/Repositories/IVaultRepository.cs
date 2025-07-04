namespace MyDiet.Identity.Domain.Repositories
{
    public interface IVaultRepository<TData, TResponse>
    {
        Task<TData?> GetDeletedSecretAsync(string secretName);
        Task<TData?> CreateSecretAsync(TData secret);
        Task<TData?> GetSecretAsync(string secretName);
        Task<TResponse?> PurgeDeletedSecretAsync(string secretName);
    }
}
