namespace NoteService.API.Entities;

public class TagNotesEntity
{
    public int TagNoteId { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<NotesEntity> Note { get; set; } = [];
}
