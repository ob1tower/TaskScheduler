using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteService.API.Entities;
using NoteService.API.Enums;

namespace NoteService.API.Configurations;

public class NotesConfiguration : IEntityTypeConfiguration<NotesEntity>
{
    public void Configure(EntityTypeBuilder<NotesEntity> builder)
    {
        builder.HasKey(x => x.NoteId);

        builder.Property(b => b.Title)
               .IsRequired();

        builder.Property(b => b.Description)
               .IsRequired(false);

        builder.Property(b => b.Status)
               .IsRequired();

        builder.Property(b => b.DueDate)
               .IsRequired();

        builder.Property(b => b.CreatedAt)
               .IsRequired();

        builder.HasOne(b => b.Project)
               .WithMany(b => b.Note)
               .HasForeignKey(b => b.ProjectId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.HasOne(b => b.TagNote)
               .WithMany(b => b.Note)
               .HasForeignKey(b => b.TagNoteId)
               .IsRequired();
    }
}