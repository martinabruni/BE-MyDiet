namespace MyDiet.Session.Domain.Responses
{
    public static class RepositoryResponseExtension
    {
        public static BusinessResponse<TData> ToBusinessResponse<TData>(this RepositoryResponse<TData> repositoryResponse)
            where TData : class
        {
            return new BusinessResponse<TData>
            {
                Data = repositoryResponse.Data,
                Message = repositoryResponse.Message,
                StatusCode = (BusinessCode)repositoryResponse.StatusCode
            };
        }
    }
}
