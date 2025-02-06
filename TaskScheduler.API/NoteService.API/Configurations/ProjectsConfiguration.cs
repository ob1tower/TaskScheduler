using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteService.API.Entities;
using NoteService.API.Enums;

namespace NoteService.API.Configurations;

public class ProjectsConfiguration : IEntityTypeConfiguration<ProjectsEntity>
{
    public void Configure(EntityTypeBuilder<ProjectsEntity> builder)
    {
        builder.HasKey(x => x.ProjectId);

        builder.Property(b => b.Name)
               .IsRequired();

        builder.Property(b => b.Description)
               .IsRequired(false);

        builder.Property(b => b.CreatedAt)
               .IsRequired();

        builder.HasMany(b => b.Note)
               .WithOne(b => b.Project)
               .HasForeignKey(b => b.ProjectId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.HasOne(b => b.TagProject)
              .WithMany(b => b.Project)
              .HasForeignKey(b => b.TagProjectId)
              .IsRequired();
    }
}