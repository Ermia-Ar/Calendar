namespace Core.Domain.Identity
{
    public static class JwtSettings
    {
        public const string Issuer = "https://localhost:7107/";
        public const string Audience = "https://localhost:7107/";
        public const string Key = "TrTrWmtTtwUe^zw131252ewW*/e#@3E$miaERMIA!r%ia098efs098af&ad8";
        public static bool ValidateIssuer;
        public static bool ValidateAudience;
        public static bool ValidateLifeTime { get; set; }
        public static bool ValidateIssuerSigningKey { get; set; }
        public static int AccessTokenExpireDate { get; set; }
        public static int RefreshTokenExpireDate { get; set; }
    }
}
