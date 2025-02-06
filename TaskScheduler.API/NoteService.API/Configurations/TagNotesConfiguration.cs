using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteService.API.Entities;
using NoteService.API.Enums;

namespace NoteService.API.Configurations;

public class TagNotesConfiguration : IEntityTypeConfiguration<TagNotesEntity>
{
    public void Configure(EntityTypeBuilder<TagNotesEntity> builder)
    {
        builder.HasKey(x => x.TagNoteId);

        builder.Property(b => b.Name)
               .IsRequired();

        builder.HasData(
                Enum
                .GetValues<NoteTag>()
                .Select(r => new TagNotesEntity
                {
                    TagNoteId = (int)r,
                    Name = r.ToString()
                }));

        builder.HasMany(b => b.Note)
               .WithOne(b => b.TagNote)
               .HasForeignKey(b => b.TagNoteId)
               .IsRequired();
    }
}
