using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Shared.Domain.Responses;
using System.Net;

namespace MyDiet.Identity.Business.Services
{
    internal abstract class AGenericJwtTokenService<TClaim> : IJwtTokenService<TClaim> where TClaim : class
    {
        private readonly IJwtTokenGenerator<TClaim> _jwtTokenGenerator;
        private readonly IKeyProvider _keyProvider;

        public AGenericJwtTokenService(IJwtTokenGenerator<TClaim> jwtTokenGenerator, IKeyProvider keyProvider)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _keyProvider = keyProvider;
        }

        public async Task<ApiDataResponse<string>> GetPublicKeyAsync()
        {
            try
            {
                return new ApiDataResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = await _keyProvider.GetPublicKeyAsync(),
                    Message = "Public key retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ApiDataResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.ToString(),
                };
            }
        }

        public async Task<ApiDataResponse<string>> GenerateTokenAsync(TClaim claimDto)
        {
            try
            {
                return new ApiDataResponse<string>
                {
                    StatusCode = HttpStatusCode.Created,
                    Data = await _jwtTokenGenerator.GenerateTokenAsync(claimDto),
                    Message = "Token generated successfully."
                };
            }
            catch (Exception ex)
            {
                return new ApiDataResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.ToString(),
                };
            }
        }
    }
}
