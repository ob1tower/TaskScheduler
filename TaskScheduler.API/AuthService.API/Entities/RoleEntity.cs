namespace AuthService.API.Entities;

public class RoleEntity
{
    public int RoleId { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<UserEntity> User { get; set; } = [];
}
