using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteService.API.Entities;
using NoteService.API.Enums;

namespace NoteService.API.Configurations;

public class TagProjectsConfiguration : IEntityTypeConfiguration<TagProjectsEntity>
{
    public void Configure(EntityTypeBuilder<TagProjectsEntity> builder)
    {
        builder.HasKey(x => x.TagProjectId);

        builder.Property(b => b.Name)
               .IsRequired();

        builder.HasData(
                Enum
                .GetValues<ProjectTag>()
                .Select(r => new TagProjectsEntity
                {
                    TagProjectId = (int)r,
                    Name = r.ToString()
                }));

        builder.HasMany(b => b.Project)
               .WithOne(b => b.TagProject)
               .HasForeignKey(b => b.TagProjectId)
               .IsRequired();
    }
}
