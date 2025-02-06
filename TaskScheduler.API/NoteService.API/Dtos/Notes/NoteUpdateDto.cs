using NoteService.API.Enums;

namespace NoteService.API.Dtos.Notes;

public record NoteUpdateDto(string Title, string? Description, NoteStatus Status,
                            int TagNoteId, DateTime DueDate, Guid ProjectId);