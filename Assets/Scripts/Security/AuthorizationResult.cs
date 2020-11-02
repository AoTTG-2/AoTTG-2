namespace Assets.Scripts.Security
{
    public class AuthorizationResult
    {
        public string AuthorizationCode { get; set; }
        public string CodeVerifier { get; set; }
        public string RedirectUrl { get; set; }
    }
}
