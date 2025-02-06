using AuthService.API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AuthService.API.Enums;

namespace AuthService.API.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(x => x.RoleId);

        builder.Property(b => b.Name)
               .IsRequired();

        builder.HasData(
                Enum
                .GetValues<UserRole>()
                .Select(r => new RoleEntity
                {
                    RoleId = (int)r,
                    Name = r.ToString()
                }));

        builder.HasMany(b => b.User)
               .WithOne(b => b.Role)
               .HasForeignKey(b => b.RoleId)
               .IsRequired();
    }
}
