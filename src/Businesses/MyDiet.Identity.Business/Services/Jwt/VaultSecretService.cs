using Azure;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Identity.Domain.Options;
using MyDiet.Identity.Domain.Repositories;
using MyDiet.Identity.Domain.Services;
using MyDiet.Shared.Domain.Mappers;
using MyDiet.Shared.Domain.Responses;
using System.Net;
using System.Security.Cryptography;

namespace MyDiet.Identity.Business.Services.Jwt
{
    internal class VaultSecretService : IJwtKeyService<RsaSecurityKey, JsonWebKeySetDto>
    {
        private readonly IVaultRepository<KeyVaultSecret, Response> _keyRepository;
        private readonly IMapper<RSA, KeyVaultSecret> _rsaToSecretMapper;
        private readonly IMapper<OpenIdOption, OpenIdConfigurationDto> _openIdMapper;
        private readonly IMapper<KeyVaultSecret, RsaSecurityKey> _secretToRsaMapper;
        private readonly IMapper<KeyVaultSecret, JsonWebKeySetDto> _privateToPublicKeyMapper;
        private readonly KeyVaultOption _keyVaultOption;
        private readonly OpenIdOption _openIdOption;
        private readonly KeyPairDto _keyPairDto;

        public VaultSecretService(IVaultRepository<KeyVaultSecret, Response> keyRepository, KeyVaultOption keyVaultOption, OpenIdOption openIdOption, KeyPairDto keyPairDto, IMapper<RSA, KeyVaultSecret> rsaToSecretMapper, IMapper<KeyVaultSecret, JsonWebKeySetDto> privateToPublicKeyMapper, IMapper<OpenIdOption, OpenIdConfigurationDto> openIdMapper, IMapper<KeyVaultSecret, RsaSecurityKey> secretToRsaMapper)
        {
            _keyRepository = keyRepository;
            _keyVaultOption = keyVaultOption;
            _openIdOption = openIdOption;
            _keyPairDto = keyPairDto;
            _rsaToSecretMapper = rsaToSecretMapper;
            _privateToPublicKeyMapper = privateToPublicKeyMapper;
            _openIdMapper = openIdMapper;
            _secretToRsaMapper = secretToRsaMapper;
        }

        private bool IsOpenIdConfigurationValid()
        {
            return !string.IsNullOrEmpty(_openIdOption.Issuer) &&
                   !string.IsNullOrEmpty(_openIdOption.TokenEndpoint) &&
                   !string.IsNullOrEmpty(_openIdOption.JwksUri) &&
                   _openIdOption.ClaimsSupported?.Count > 0;
        }

        public async Task<ApiResponse<RsaSecurityKey>> RegenerateKeyPairAsync()
        {
            try
            {
                if (_keyVaultOption.PrivateKeyName is null)
                {
                    return new ApiResponse<RsaSecurityKey>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Key vault secret name is not configured."
                    };
                }

                var privateKey = await _keyRepository.GetSecretAsync(_keyVaultOption.PrivateKeyName);

                if (privateKey is not null)
                {
                    _keyPairDto.PrivateKey = privateKey;
                    _keyPairDto.PublicKey = _privateToPublicKeyMapper.Map(privateKey);

                    return new ApiResponse<RsaSecurityKey>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Keys updated successfully."
                    };
                }
                return await CreateKeyPairAsync();
            }
            catch (RequestFailedException)
            {
                return await CreateKeyPairAsync();
            }
            catch
            {
                return new ApiResponse<RsaSecurityKey>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Error creating key"
                };
            }
        }

        public ApiResponse<OpenIdConfigurationDto> GetOpenIdConfigurationKey()
        {
            if (IsOpenIdConfigurationValid() is false)
            {
                return new ApiResponse<OpenIdConfigurationDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred while retrieving the OpenID configuration.",
                };
            }

            return new ApiResponse<OpenIdConfigurationDto>()
            {
                Data = _openIdMapper.Map(_openIdOption),
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ApiResponse<RsaSecurityKey> GetPrivateKey()
        {
            try
            {
                if (_keyPairDto.PrivateKey is null)
                {
                    return new ApiResponse<RsaSecurityKey>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Key not found",
                    };
                }

                return new ApiResponse<RsaSecurityKey>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Key retrieved successfully.",
                    Data = _secretToRsaMapper.Map(_keyPairDto.PrivateKey)
                };
            }
            catch
            {
                return new ApiResponse<RsaSecurityKey>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Error retrieving key",
                };
            }
        }

        public ApiResponse<JsonWebKeySetDto> GetPublicKey()
        {
            try
            {
                if (_keyPairDto.PrivateKey is null)
                {
                    return new ApiResponse<JsonWebKeySetDto>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Key not found",
                    };
                }

                var publicKey = _keyPairDto.PublicKey;
                if (publicKey is null)
                {
                    return new ApiResponse<JsonWebKeySetDto>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Key not found",
                    };
                }

                return new ApiResponse<JsonWebKeySetDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Key retrieved successfully.",
                    Data = publicKey
                };
            }
            catch
            {
                return new ApiResponse<JsonWebKeySetDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Error retrieving key vault secret",
                };
            }
        }

        public async Task<ApiResponse<RsaSecurityKey>> CreateKeyPairAsync()
        {
            var secret = _rsaToSecretMapper.Map(RSA.Create(_keyVaultOption.KeySize));

            if (secret is null)
            {
                return new ApiResponse<RsaSecurityKey>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to create RSA key."
                };
            }

            try
            {
                var isDeleted = await _keyRepository.GetDeletedSecretAsync(_keyVaultOption.PrivateKeyName);

                if (isDeleted is not null)
                {
                    await _keyRepository.PurgeDeletedSecretAsync(_keyVaultOption.PrivateKeyName);
                }
                secret = await _keyRepository.CreateSecretAsync(secret);
            }
            catch
            {
                secret = await _keyRepository.CreateSecretAsync(secret);
            }

            _keyPairDto.PrivateKey = secret;
            _keyPairDto.PublicKey = _privateToPublicKeyMapper.Map(secret);

            return new ApiResponse<RsaSecurityKey>()
            {
                StatusCode = HttpStatusCode.Created,
                Message = "Key stored successfully."
            };
        }
    }
}
