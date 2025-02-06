namespace AuthService.API.Models;

public class User
{
    public Guid UserId { get; }
    public string UserName { get; } = string.Empty;
    public string Email { get; } = string.Empty;
    public string PasswordHash { get; } = string.Empty;
    public DateTime CreatedAt { get; }

    public int RoleId { get; }
    public Role? Role { get; }
    public List<RefreshToken> RefreshTokens { get; set; } = [];

    public User(Guid userId, string userName, string email, string passwordHash)
    {
        UserId = userId;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }
}
