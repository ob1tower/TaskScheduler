using AuthService.API.Configurations;
using AuthService.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.DataAccess;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
}
