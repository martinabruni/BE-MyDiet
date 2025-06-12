using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Shared.Domain.Responses;
using System.Net;
using System.Security.Cryptography;

namespace MyDiet.Identity.Business.Services
{
    internal abstract class AGenericJwtTokenService<TClaim, TKey> : IJwtTokenService<TClaim, TKey> where TKey : AsymmetricAlgorithm where TClaim : class
    {
        private readonly IJwtTokenGenerator<TClaim> _jwtTokenGenerator;
        private readonly IKeyProvider<TKey> _keyProvider;

        public AGenericJwtTokenService(IJwtTokenGenerator<TClaim> jwtTokenGenerator, IKeyProvider<TKey> keyProvider)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _keyProvider = keyProvider;
        }

        public async Task<ApiDataResponse<string>> GetPemPublicKey()
        {
            try
            {
                TKey rsa = await _keyProvider.GetPrivateKeyAsync();
                var publicKey = rsa.ExportSubjectPublicKeyInfo();
                var pemPublicKey = PemEncoding.WriteString("PUBLIC KEY", publicKey);
                return new ApiDataResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = pemPublicKey,
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

        public async Task<ApiDataResponse<string>> GetPublicKeyAsync()
        {
            try
            {
                TKey rsa = await _keyProvider.GetPrivateKeyAsync();
                var publicKey = rsa.ExportSubjectPublicKeyInfo();
                return new ApiDataResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = Convert.ToBase64String(publicKey),
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
