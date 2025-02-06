using NoteService.API.Dtos.Notes;

namespace NoteService.API.Dtos.Projects;
public record ProjectWithNoteGetDto(Guid ProjectId, string Name, string? Description,
                            int TagProjectId, DateTime CreatedAt, List<NoteGetDto> Notes);
