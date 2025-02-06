using CSharpFunctionalExtensions;
using NoteService.API.Dtos.Projects;

namespace NoteService.API.Services.Interfaces
{
    public interface IProjectsService
    {
        Task<Result<Guid>> CreateProjects(ProjectCreateDto projectCreateDto);
        Task<Result> DeleteProjects(Guid id);
        Task<Result<ProjectWithNoteGetDto>> GetProject(Guid id);
        Task<Result<List<ProjectGetDto>>> GetAllProjects();
        Task<Result<Guid>> UpdateProjects(Guid id, ProjectUpdateDto projectUpdateDto);
    }
}