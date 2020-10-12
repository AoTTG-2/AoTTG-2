using UnityEngine;

namespace Assets.Scripts.Security
{

    // Based on: https://developer.okta.com/blog/2020/08/21/unity-csharp-games-security
    [CreateAssetMenu, ExecuteInEditMode]
    public class OAuth : ScriptableObject
    {
        public string clientID;
        public string clientSecret;
        public string authorizationEndpoint;
        public string tokenEndpoint;
        public string userInfoEndpoint;
        public string HealthCheckEndpoint;
        }
}
