namespace NoteService.API.Entities;

public class TagProjectsEntity
{
    public int TagProjectId { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<ProjectsEntity> Project { get; set; } = [];
}
