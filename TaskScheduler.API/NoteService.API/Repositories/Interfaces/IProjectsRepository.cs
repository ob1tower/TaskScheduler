using NoteService.API.Enums;
using NoteService.API.Models;

namespace NoteService.API.Repositories.Interfaces
{
    public interface IProjectsRepository
    {
        Task<Guid> CreateProjects(Project projects);
        Task<Guid> DeleteProjects(Guid id);
        Task<Project> GetProject(Guid id);
        Task<List<Project>> GetProjects();
        Task<Guid> UpdateProjects(Guid projectId, string name, string? description, int tagProjectId);
        Task<Project> SearchTitle(string name);
    }
}