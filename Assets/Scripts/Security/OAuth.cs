using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Security
{

    // Based on: https://developer.okta.com/blog/2020/08/21/unity-csharp-games-security
    /// <summary>
    /// ScriptableObject which contains configuration for an OAuth client. <see href="https://github.com/AoTTG-2/AoTTG-2-API"></see>
    /// </summary>
    [CreateAssetMenu, ExecuteInEditMode]
    public class OAuth : ScriptableObject
    {
        public string ClientId;
        public string ClientSecret;
        public string Endpoint;

        public string AuthorizationEndpoint { get; private set; }
        public string TokenEndpoint { get; private set; }
        public string UserInfoEndpoint { get; private set; }
        public string EndSessionEndpoint { get; private set; }

        public async Task SetEndpointsViaDiscoveryDocumentAsync()
        {
            var client = new HttpClient();
            var url = $"{Endpoint}/.well-known/openid-configuration";

            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            AuthorizationEndpoint = result["authorization_endpoint"] as string;
            TokenEndpoint = result["token_endpoint"] as string;
            UserInfoEndpoint = result["userinfo_endpoint"] as string;
            EndSessionEndpoint = result["end_session_endpoint"] as string;
        }

        public string GetHealthCheckEndpoint()
        {
            return Endpoint + "/health";
        }
    }
}
