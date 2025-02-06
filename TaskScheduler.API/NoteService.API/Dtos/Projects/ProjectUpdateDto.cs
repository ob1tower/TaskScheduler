using NoteService.API.Enums;

namespace NoteService.API.Dtos.Projects;

public record ProjectUpdateDto(string Name, string? Description, int TagProjectId);
