using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Business.Converters;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Shared.Domain.Responses;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;

namespace MyDiet.Identity.Business.Services
{
    internal class KeyVaultSecretService : IJwtKeyService<RsaSecurityKey, JwkSetDto>
    {
        private readonly RSA _rsa;
        private readonly ByteArrayBase64Converter _converter;
        private readonly JsonSerializerOptions _options;
        private readonly IJwtKeyRepository<KeyVaultSecret> _keyRepository;

        public KeyVaultSecretService(RSA rsa, ByteArrayBase64Converter converter, IJwtKeyRepository<KeyVaultSecret> keyRepository)
        {
            _rsa = rsa;
            _converter = converter;
            _keyRepository = keyRepository;
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };
            _options.Converters.Add(_converter);
        }

        //TODO: fix return type
        public async Task<ApiResponse<RsaSecurityKey>> CreatePrivateKeyAsync()
        {
            try
            {
                RSAParameters parameters = _rsa.ExportParameters(true);
                string serializedParameters = JsonSerializer.Serialize(parameters, _options);
                KeyVaultSecret secret = new KeyVaultSecret("privateKey", serializedParameters)
                {
                    Properties =
                {
                    ContentType = "application/json"
                }
                };
                await _keyRepository.CreatePrivateKeyAsync(secret);
                return new ApiResponse<RsaSecurityKey>()
                {
                    StatusCode = HttpStatusCode.Created,
                    Message = "Secret stored successfully."
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<RsaSecurityKey>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Error creating key vault secret: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<JwkSetDto>> GetPublicKeyAsync()
        {
            try
            {
                var secret = await _keyRepository.GetPublicKeyAsync();
                if (secret is null)
                {
                    return new ApiResponse<JwkSetDto>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = $"Key vault secret is null",
                        Data = null
                    };
                }
                RSAParameters deserializedParameters = JsonSerializer.Deserialize<RSAParameters>(secret.Value, _options);
                var rsa = RSA.Create(2048);
                rsa.ImportParameters(deserializedParameters);

                return new ApiResponse<JwkSetDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Secret retrieved successfully.",
                    Data = new JwkSetDto
                    {
                        Keys = new List<JwkDto>
                        {
                            new JwkDto
                            {
                                Kty = "RSA",
                                Use = "sig",
                                Kid = new RsaSecurityKey(rsa).KeyId,
                                Alg = "RS256",
                                N = Base64UrlEncoder.Encode(deserializedParameters.Modulus),
                                E = Base64UrlEncoder.Encode(deserializedParameters.Exponent)
                            }
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<JwkSetDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Error retrieving key vault secret: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}