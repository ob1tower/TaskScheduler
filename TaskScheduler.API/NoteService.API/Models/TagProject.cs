namespace NoteService.API.Models;

public class TagProject
{
    public int TagProjectId { get; }
    public string Name { get; }

    public Project? Project { get; set; }

    public TagProject(int tagProjectId, string name)
    {
        TagProjectId = tagProjectId;
        Name = name;
    }
}
