namespace ImageGallery.Contracts.Models.Options
{
    public sealed class JwtOptions
    {
        public const string JWT_OPTIONS = "JWT";

        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public bool ValidateLifetime { get; set; }
        public int Lifetime { get; set; }
        public int ClockSkew { get; set; }
    }
}
