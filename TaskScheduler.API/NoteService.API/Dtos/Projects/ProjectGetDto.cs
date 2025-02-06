using NoteService.API.Enums;

namespace NoteService.API.Dtos.Projects;

public record ProjectGetDto(Guid ProjectId, string Name, string? Description, 
                            int TagProjectId, DateTime CreatedAt);