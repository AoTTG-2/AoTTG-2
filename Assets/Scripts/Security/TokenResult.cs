using Newtonsoft.Json;

namespace Assets.Scripts.Security
{
    /// <summary>
    /// An object for the OAuth Token endpoint
    /// </summary>
    public class TokenResult
    {

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
