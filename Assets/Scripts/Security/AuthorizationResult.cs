namespace Assets.Scripts.Security
{
    /// <summary>
    /// An object for the response of an OAuth authorization code endpoint
    /// </summary>
    public class AuthorizationResult
    {
        public string AuthorizationCode { get; set; }
        public string CodeVerifier { get; set; }
        public string RedirectUrl { get; set; }
    }
}
