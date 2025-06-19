using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos;
using MyDiet.Identity.Domain.Interfaces;
using MyDiet.Identity.Domain.Options;
using MyDiet.Shared.Domain.Responses;
using MyDiet.Shared.Infrastructure.Converters;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;

namespace MyDiet.Identity.Business.Services
{
    internal class KeyVaultSecretService : IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto>
    {
        private readonly RSA _rsa;
        private readonly ByteArrayBase64Converter _converter;
        private readonly JsonSerializerOptions _options;
        private readonly IJwtKeyRepository<KeyVaultSecret> _keyRepository;
        private readonly KeyVaultOption _keyVaultOption;
        private readonly TokenOption _tokenOption;
        private readonly OpenIdOption _openIdOption;
        public KeyVaultSecretService(RSA rsa, ByteArrayBase64Converter converter, IJwtKeyRepository<KeyVaultSecret> keyRepository, KeyVaultOption keyVaultOption, TokenOption tokenOption, OpenIdOption openIdOption)
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
            _keyVaultOption = keyVaultOption;
            _tokenOption = tokenOption;
            _openIdOption = openIdOption;
        }

        public async Task<ApiResponse<RsaSecurityKey>> CreatePrivateKeyAsync()
        {
            try
            {
                RSAParameters parameters = _rsa.ExportParameters(true);
                string serializedParameters = JsonSerializer.Serialize(parameters, _options);
                KeyVaultSecret secret = new KeyVaultSecret(_keyVaultOption.SecretName, serializedParameters)
                {
                    Properties =
                    {
                        ContentType = "application/json",
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

        public Task<ApiResponse<OpenIdConfigurationDto>> GetOpenIdConfigurationKeyAsync()
        {
            var openIdConfiguration = new OpenIdConfigurationDto()
            {
                Issuer = _tokenOption.Issuer,
                AuthorizationEndpoint = _openIdOption.AuthorizationEndpoint,
                TokenEndpoint = _openIdOption.TokenEndpoint,
                JwksUri = _openIdOption.JwksUri,
                IdTokenSigningAlgorithms = _openIdOption.IdTokenSigningAlgorithms,
                ClaimsSupported = _openIdOption.ClaimsSupported,
            };
            return Task.FromResult(new ApiResponse<OpenIdConfigurationDto>()
            {
                Data = openIdConfiguration,
                StatusCode = HttpStatusCode.OK,
            });
        }

        public async Task<ApiResponse<RsaSecurityKey>> GetPrivateKeyAsync()
        {
            try
            {
                var secret = await _keyRepository.GetPrivateKeyAsync();
                if (secret is null)
                {
                    return new ApiResponse<RsaSecurityKey>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = $"Key vault secret is null",
                        Data = null
                    };
                }
                RSAParameters deserializedParameters = JsonSerializer.Deserialize<RSAParameters>(secret.Value, _options);

                return new ApiResponse<RsaSecurityKey>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Secret retrieved successfully.",
                    Data = new RsaSecurityKey(deserializedParameters)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<RsaSecurityKey>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Error retrieving key vault secret: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<JsonWebKeySetDto>> GetPublicKeyAsync()
        {
            try
            {
                var secret = await _keyRepository.GetPrivateKeyAsync();
                if (secret is null)
                {
                    return new ApiResponse<JsonWebKeySetDto>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = $"Key vault secret is null",
                        Data = null
                    };
                }
                RSAParameters deserializedParameters = JsonSerializer.Deserialize<RSAParameters>(secret.Value, _options);
                var rsa = RSA.Create(2048);
                rsa.ImportParameters(deserializedParameters);

                return new ApiResponse<JsonWebKeySetDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Secret retrieved successfully.",
                    Data = new JsonWebKeySetDto
                    {
                        Keys = new List<JsonWebKeyDto>
                        {
                            new JsonWebKeyDto
                            {
                                Kty = "RSA",
                                Use = "sig",
                                Kid = secret.Properties.Version,
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
                return new ApiResponse<JsonWebKeySetDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Error retrieving key vault secret: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}