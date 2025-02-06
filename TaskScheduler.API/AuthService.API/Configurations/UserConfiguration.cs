using AuthService.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.API.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property(b => b.Email)
               .IsRequired();

        builder.Property(b => b.PasswordHash)
               .IsRequired();

        builder.Property(b => b.CreatedAt)
               .IsRequired();

        builder.HasMany(b => b.RefreshToken)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId)
               .IsRequired();

        builder.HasOne(b => b.Role)
               .WithMany(b => b.User)
               .HasForeignKey(b => b.RoleId)
               .IsRequired();
    }
}
