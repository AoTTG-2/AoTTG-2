using Assets.Scripts.Security;
using Newtonsoft.Json;
using Photon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class AuthenticationService : PunBehaviour
    {
        public OAuth OAuth;

        private string accessToken;
        public string AccessToken
        {
            get
            {
                if (RefreshToken != null)
                {
                    var validTimer = AccessTokenExpiration - DateTime.UtcNow;
                    if (validTimer.Minutes < 5)
                    {
                        Task.Run(RefreshAccessToken);
                    }
                }

                return accessToken;
            }
            set => accessToken = value;
        }
        private string IdToken { get; set; }

        /// <summary>
        /// The Refresh Token. Valid for 30 days, can be used to request a new Access Token without requiring user authentication. Value of the RefreshToken changes per refresh
        /// </summary>
        private string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }

        private const string CodeChallengeMethod = "S256";

        private async void Start()
        {
            await OAuth.SetEndpointsViaDiscoveryDocumentAsync();
        }

        /// <summary>
        /// Attempts to login the user with the configured <see cref="OAuth"/> Identity Provider
        /// </summary>
        /// <returns>A bool indicating if the login attempt was successful</returns>
        public async Task<bool> LoginAsync()
        {
            var authorizationResult = await GetAuthorizationCode();
            if (authorizationResult == null)
            {
                Debug.LogError("Could not receive Authorization Code.");
                return false;
            }

            var accessTokenResult = await SetAccessToken(authorizationResult);
            if (!accessTokenResult)
            {
                Debug.LogError("Could not set the access token");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to logout the user from the configured <see cref="OAuth"/> Identity Provider
        /// </summary>
        /// <returns>A bool indicating if the logout attempt was successful</returns>
        public async Task<bool> LogoutAsync()
        {
            var requestUri = $"{OAuth.EndSessionEndpoint}" +
                             $"?id_token_hint={IdToken}";
            System.Diagnostics.Process.Start(requestUri);
            return true;
        }

        /// <summary>
        /// Returns a HttpResponseMessage containing HealthCheck information
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetHealthCheckResponse()
        {
            var client = new HttpClient();
            return await client.GetAsync(OAuth.GetHealthCheckEndpoint());
        }

        private async Task<AuthorizationResult> GetAuthorizationCode()
        {
            var state = RandomDataBase64Url(32);
            var codeVerifier = RandomDataBase64Url(32);
            var codeChallenge = Base64UrlEncodeNoPadding(Sha256(codeVerifier));

            var redirectUri = $"http://{IPAddress.Loopback}:{51772}/";

            var http = new HttpListener();
            http.Prefixes.Add(redirectUri);
            http.Start();

            var authorizationRequest =
                $"{OAuth.AuthorizationEndpoint}?response_type=code" +
                $"&scope=openid%20profile%20offline_access" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                $"&client_id={OAuth.ClientId}" +
                $"&state={state}" +
                $"&code_challenge={codeChallenge}" +
                $"&code_challenge_method={CodeChallengeMethod}";

            //TODO: Investigate how to use an in-game browser
            System.Diagnostics.Process.Start(authorizationRequest);

            var context = await http.GetContextAsync();
            // Sends an HTTP response to the browser.
            var response = context.Response;
            string responseString = string.Format("<html><head></head><body>Please return to AoTTG2.</body></html>");
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
            {
                responseOutput.Close();
                http.Stop();
                Console.WriteLine("HTTP server stopped.");
            });

            // Checks for errors.
            if (context.Request.QueryString.Get("error") != null)
            {
                Debug.LogWarning($"OAuth authorization error: {context.Request.QueryString.Get("error")}.");
                return null;
            }
            if (context.Request.QueryString.Get("code") == null
                || context.Request.QueryString.Get("state") == null)
            {
                Debug.LogWarning("Malformed authorization response. " + context.Request.QueryString);
                return null;
            }

            // extracts the code
            var code = context.Request.QueryString.Get("code");
            var incoming_state = context.Request.QueryString.Get("state");
            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization.
            if (incoming_state != state)
            {
                Debug.LogWarning(String.Format("Received request with invalid state ({0})", incoming_state));
                return null;
            }

            return new AuthorizationResult
            {
                AuthorizationCode = code,
                CodeVerifier = codeVerifier,
                RedirectUrl = redirectUri
            };
        }

        private async Task<bool> RefreshAccessToken()
        {
            var client = new HttpClient();
            var requestBody = new Dictionary<string, string>
            {
                { "client_id", OAuth.ClientId },
                { "client_secret", OAuth.ClientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", RefreshToken }
            };
            var request = new HttpRequestMessage(HttpMethod.Post, OAuth.TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(requestBody)
            };

            var result = await client.SendAsync(request);
            if (!result.IsSuccessStatusCode) return false;

            var content = await result.Content.ReadAsStringAsync();
            var tokenResult = JsonConvert.DeserializeObject<TokenResult>(content);
            AccessToken = tokenResult.AccessToken;
            IdToken = tokenResult.IdToken;
            RefreshToken = tokenResult.RefreshToken;
            AccessTokenExpiration = DateTime.UtcNow.AddSeconds(tokenResult.ExpiresIn);

            return true;
        }

        private async Task<bool> SetAccessToken(AuthorizationResult authorizationResult)
        {
            // builds the  request
            var tokenRequestBody =
                $"code={authorizationResult.AuthorizationCode}" +
                $"&redirect_uri={Uri.EscapeDataString(authorizationResult.RedirectUrl)}" +
                $"&client_id={OAuth.ClientId}" +
                $"&code_verifier={authorizationResult.CodeVerifier}" +
                $"&client_secret={OAuth.ClientSecret}" +
                $"&grant_type=authorization_code";

            // sends the request
            HttpWebRequest tokenRequest = (HttpWebRequest) WebRequest.Create(OAuth.TokenEndpoint);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";

            byte[] _byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = _byteVersion.Length;
            Stream stream = tokenRequest.GetRequestStream();
            await stream.WriteAsync(_byteVersion, 0, _byteVersion.Length);
            stream.Close();
            try
            {
                // gets the response
                WebResponse tokenResponse = await tokenRequest.GetResponseAsync();
                using (StreamReader reader = new StreamReader(tokenResponse.GetResponseStream()))
                {
                    // reads response body
                    var responseText = await reader.ReadToEndAsync();
                    var tokenResult = JsonConvert.DeserializeObject<TokenResult>(responseText);
                    AccessToken = tokenResult.AccessToken;
                    IdToken = tokenResult.IdToken;
                    RefreshToken = tokenResult.RefreshToken;
                    AccessTokenExpiration = DateTime.UtcNow.AddSeconds(tokenResult.ExpiresIn);
                    return true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.ProtocolError) return false;
                if (!(ex.Response is HttpWebResponse response)) return false;

                Debug.LogWarning("HTTP: " + response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // reads response body
                    string responseText = await reader.ReadToEndAsync();
                    Debug.LogWarning(responseText);
                }
            }

            return false;
        }


        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        /// <returns></returns>
        private static string RandomDataBase64Url(uint length)
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return Base64UrlEncodeNoPadding(bytes);
        }

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static string Base64UrlEncodeNoPadding(byte[] buffer)
        {
            var base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }

        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static byte[] Sha256(string inputString)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputString);
            SHA256Managed sha256 = new SHA256Managed();
            return sha256.ComputeHash(bytes);
        }

        private async Task userinfoCall(string access_token)
        {
            // sends the request
            HttpWebRequest userinfoRequest = (HttpWebRequest) WebRequest.Create(OAuth.UserInfoEndpoint);
            userinfoRequest.Method = "GET";
            userinfoRequest.Headers.Add(string.Format("Authorization: Bearer {0}", access_token));
            userinfoRequest.ContentType = "application/x-www-form-urlencoded";

            // gets the response
            WebResponse userinfoResponse = await userinfoRequest.GetResponseAsync();
            using (StreamReader userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream()))
            {
                // reads response body
                string userinfoResponseText = await userinfoResponseReader.ReadToEndAsync();
                Debug.Log(userinfoResponseText);
            }
        }
    }
}
