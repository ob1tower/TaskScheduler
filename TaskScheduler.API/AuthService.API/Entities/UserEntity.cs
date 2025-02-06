namespace AuthService.API.Entities;

public class UserEntity
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public int RoleId { get; set; }
    public RoleEntity? Role { get; set; }
    public List<RefreshTokenEntity> RefreshToken { get; set; } = [];
}
