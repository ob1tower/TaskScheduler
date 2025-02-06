using NoteService.API.Enums;

namespace NoteService.API.Dtos.Notes;

public record NoteGetDto(Guid NoteId, string Title, string? Description, NoteStatus Status,
                         int TagNoteId, DateTime DueDate, DateTime CreatedAt);