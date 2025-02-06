using AuthService.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.API.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(x => x.Token);

        builder.Property(b => b.CreatedAt)
               .IsRequired();

        builder.Property(b => b.Expires)
               .IsRequired();

        builder.HasOne(b => b.User)
               .WithMany(b => b.RefreshToken)
               .HasForeignKey(b => b.UserId)
               .IsRequired();
    }
}