using NoteService.API.Enums;

namespace NoteService.API.Models;

public class Note
{
    public Guid NoteId { get; }
    public string Title { get; } = string.Empty;
    public string? Description { get; }
    public NoteStatus Status { get; }
    public DateTime DueDate { get; }
    public DateTime CreatedAt { get; }

    public Guid ProjectId { get; }
    public Project? Project { get; }

    public int TagNoteId { get; }
    public TagNote? TagNote { get; }

    public Note(Guid noteId, string title, string? description, NoteStatus status, int tagNoteId, DateTime dueDate, Guid projectId)
    {
        NoteId = noteId;
        Title = title;
        Description = description;
        Status = status;
        TagNoteId = tagNoteId;
        DueDate = dueDate;
        ProjectId = projectId;
        CreatedAt = DateTime.UtcNow;
    }
}
