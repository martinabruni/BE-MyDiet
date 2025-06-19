namespace MyDiet.Identity.Domain.Dtos
{
    public class OpenIdConfigurationDto
    {
        /// <summary>
        /// Identifica l’issuer dei token (deve terminare con “/”).
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// URL per richiedere il codice o il token (grant_type=authorization_code, etc.).
        /// </summary>
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// URL per lo scambio del codice con l’access token.
        /// </summary>
        public string TokenEndpoint { get; set; }

        /// <summary>
        /// URL da cui ottenere il JWKS (JSON Web Key Set) per validare le firme.
        /// </summary>
        public string JwksUri { get; set; }

        /// <summary>
        /// Algoritmi di firma supportati per l’ID Token (tipicamente almeno "RS256").
        /// </summary>
        public List<string> IdTokenSigningAlgorithms { get; set; }

        /// <summary>
        /// Elenco dei claims che il provider può includere nell’ID Token / UserInfo.
        /// </summary>
        public List<string> ClaimsSupported { get; set; }
    }
}
