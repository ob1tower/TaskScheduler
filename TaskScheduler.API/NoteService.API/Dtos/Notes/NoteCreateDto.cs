namespace NoteService.API.Dtos.Notes;

public record NoteCreateDto(string Title, Guid ProjectId);
