namespace AuthService.API.Models;

public class RefreshToken
{
    public Guid RefreshTokenId { get; set; }
    public string Token { get; } = string.Empty;
    public DateTime CreatedAt { get; }
    public DateTime Expires { get; }

    public Guid UserId { get; set; }
    public User? User { get; }

    public RefreshToken(string token, DateTime expires)
    {
        Token = token;
        CreatedAt = DateTime.UtcNow;
        Expires = expires;
    }
}