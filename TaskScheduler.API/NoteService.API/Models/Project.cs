namespace NoteService.API.Models;
public class Project
{
    public Guid ProjectId { get; }
    public string Name { get; } = string.Empty;
    public string? Description { get; }
    public DateTime CreatedAt { get; }

    public List<Note> Notes { get; set; } = [];

    public int TagProjectId { get; }
    public TagProject? TagProject { get; }

    public Project(Guid projectId, string name, string? description, int tagProjectId)
    {
        ProjectId = projectId;
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        TagProjectId = tagProjectId;
    }
}
