using Azure.Security.KeyVault.Secrets;
using BaseUtility;
using Microsoft.IdentityModel.Tokens;
using MyDiet.Auth.Domain.Dtos;
using MyDiet.Auth.Domain.Options;
using System.Security.Cryptography;
using System.Text.Json;

namespace MyDiet.Auth.Business.Mappers
{
    //TODO: check if this mapper can be split into multiple mappers for better separation of concerns
    internal class KeyPairMapper : IMapper<KeyVaultSecret, JsonWebKeySetDto>, IMapper<RSA, KeyVaultSecret>, IMapper<KeyVaultSecret, RsaSecurityKey>, IMapper<JsonWebKeySetDto, IEnumerable<RsaSecurityKey>>
    {
        private readonly ByteArrayBase64Converter _converter;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly JsonWebKeyOption _jwkOption;
        private readonly KeyOption _keyOption;

        public KeyPairMapper(ByteArrayBase64Converter converter, JsonWebKeyOption jwkOption, KeyOption keyOption)
        {
            _converter = converter;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };
            _jsonSerializerOptions.Converters.Add(_converter);
            _jwkOption = jwkOption;
            _keyOption = keyOption;
        }

        public JsonWebKeySetDto Map(KeyVaultSecret input)
        {
            RSAParameters deserializedParameters = JsonSerializer.Deserialize<RSAParameters>(input.Value, _jsonSerializerOptions);
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
            return new KeyVaultSecret(_keyOption.PrivateKeyName, serializedParameters)
            {
                Properties =
                {
                    ContentType = "application/json",
                }
            };
        }

        public IEnumerable<RsaSecurityKey> Map(JsonWebKeySetDto input)
        {
            if (input == null || input.Keys == null)
                yield break;

            foreach (var keyDto in input.Keys)
            {
                var modulus = Base64UrlEncoder.DecodeBytes(keyDto.N);
                var exponent = Base64UrlEncoder.DecodeBytes(keyDto.E);

                var rsaParameters = new RSAParameters
                {
                    Modulus = modulus,
                    Exponent = exponent
                };

                var rsaSecurityKey = new RsaSecurityKey(rsaParameters)
                {
                    KeyId = keyDto.Kid
                };

                yield return rsaSecurityKey;
            }
        }

        RsaSecurityKey IMapper<KeyVaultSecret, RsaSecurityKey>.Map(KeyVaultSecret input)
        {
            RSAParameters deserializedParameters = JsonSerializer.Deserialize<RSAParameters>(input.Value, _jsonSerializerOptions);
            return new RsaSecurityKey(deserializedParameters) { KeyId = input.Properties.Version };
        }
    }
}
