namespace AuthService.API.Services;

public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public double AccessTokenExpires { get; set; }
    public double RefreshTokenExpires { get; set; }
}