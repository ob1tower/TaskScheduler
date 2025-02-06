using NoteService.API.Enums;

namespace NoteService.API.Entities;

public class ProjectsEntity
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<NotesEntity> Note { get; set; } = [];

    public int TagProjectId { get; set; }
    public TagProjectsEntity? TagProject { get; set; }
}
