using BaseUtility;

namespace MyDiet.Session.Domain.Repositories
{
    public interface IVaultRepository<TData>
    {
        Task<RepositoryResponse<TData>> GetDeletedSecretAsync(string secretName);
        Task<RepositoryResponse<TData>> CreateSecretAsync(TData secret);
        Task<RepositoryResponse<TData>> GetSecretAsync(string secretName);
        Task<RepositoryResponse<TData>> PurgeSecretAsync(string secretName);
    }
}

