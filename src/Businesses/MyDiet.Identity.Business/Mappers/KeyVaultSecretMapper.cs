using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Identity.Domain.Dtos.Jwt;
using MyDiet.Identity.Domain.Options;
using MyDiet.Shared.Domain.Mappers;
using System.Security.Cryptography;
using System.Text.Json;

namespace MyDiet.Identity.Business.Mappers
{
    internal class KeyVaultSecretMapper : IMapper<KeyVaultSecret, JsonWebKeySetDto>, IMapper<RSA, KeyVaultSecret>, IMapper<KeyVaultSecret, RsaSecurityKey>
    {
        private readonly ByteArrayBase64Converter _converter;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly KeyVaultOption _keyVaultOption;
        private readonly JwkOption _jwkOption;

        public KeyVaultSecretMapper(ByteArrayBase64Converter converter, KeyVaultOption keyVaultOption, JwkOption jwkOption)
        {
            _converter = converter;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };
            _jsonSerializerOptions.Converters.Add(_converter);
            _keyVaultOption = keyVaultOption;
            _jwkOption = jwkOption;
        }

        public JsonWebKeySetDto Map(KeyVaultSecret input)
        {
            RSAParameters deserializedParameters = JsonSerializer.Deserialize<RSAParameters>(input.Value, _jsonSerializerOptions);
            var rsa = RSA.Create(_keyVaultOption.KeySize);
            rsa.ImportParameters(deserializedParameters);
            return new JsonWebKeySetDto
            {
                Keys =
                [
                    new JsonWebKeyDto
                {
                    Kty = _jwkOption.Kty,
                    Use = _jwkOption.Use,
                    Kid = input.Properties.Version,
                    Alg = _jwkOption.Alg,
                    N = Base64UrlEncoder.Encode(deserializedParameters.Modulus),
                    E = Base64UrlEncoder.Encode(deserializedParameters.Exponent)
                }
                ]
            };
        }

        public KeyVaultSecret Map(RSA input)
        {
            RSAParameters parameters = input.ExportParameters(true);
            string serializedParameters = JsonSerializer.Serialize(parameters, _jsonSerializerOptions);
            return new KeyVaultSecret(_keyVaultOption.PrivateKeyName, serializedParameters)
            {
                Properties =
                {
                    ContentType = "application/json",
                }
            };
        }

        RsaSecurityKey IMapper<KeyVaultSecret, RsaSecurityKey>.Map(KeyVaultSecret input)
        {
            RSAParameters deserializedParameters = JsonSerializer.Deserialize<RSAParameters>(input.Value, _jsonSerializerOptions);
            return new RsaSecurityKey(deserializedParameters);
        }
    }
}
