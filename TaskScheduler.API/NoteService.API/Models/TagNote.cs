namespace NoteService.API.Models;

public class TagNote
{
    public int TagNoteId { get; }
    public string Name { get; }

    public Note? Note { get; set; }

    public TagNote(int tagNoteId, string name)
    {
        TagNoteId = tagNoteId;
        Name = name;
    }
}
