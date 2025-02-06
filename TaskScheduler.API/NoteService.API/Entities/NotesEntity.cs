using NoteService.API.Enums;

namespace NoteService.API.Entities;

public class NotesEntity
{
    public Guid NoteId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public NoteStatus Status { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid ProjectId { get; set; }
    public ProjectsEntity? Project { get; set; }

    public int TagNoteId { get; set; }
    public TagNotesEntity? TagNote { get; set; }
}
