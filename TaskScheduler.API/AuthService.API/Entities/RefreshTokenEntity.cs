namespace AuthService.API.Entities;

public class RefreshTokenEntity
{
    public Guid RefreshTokenId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime Expires { get; set; }

    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
