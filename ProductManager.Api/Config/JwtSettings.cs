namespace ProductManager.Api.Config
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInHours { get; set; } = 3600;
    }
}
