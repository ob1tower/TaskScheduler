namespace AuthService.API.Models;

public class Role
{
    public int RoleId { get; }
    public string Name { get; }

    public List<User> Users { get; set; } = [];

    public Role(int roleId, string name)
    {
        RoleId = roleId;
        Name = name;
    }
}
