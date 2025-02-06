using Microsoft.EntityFrameworkCore;
using NoteService.API.DataAccess;
using NoteService.API.Entities;
using NoteService.API.Enums;
using NoteService.API.Models;
using NoteService.API.Repositories.Interfaces;

namespace NoteService.API.Repositories;

public class ProjectsRepository : IProjectsRepository
{
    private readonly NotesDbContext _context;

    public ProjectsRepository(NotesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetProjects()
    {
        var projectsEntities = await _context.Projects
            .AsNoTracking()
            .ToListAsync();

        var projects = projectsEntities
            .Select(b => new Project(b.ProjectId, b.Name, b.Description, (int)b.TagProjectId))
            .ToList();

        return projects;
    }

    public async Task<Project> GetProject(Guid id)
    {
        var projectsEntities = await _context.Projects
            .AsNoTracking()
            .Include(p => p.Note)
            .FirstOrDefaultAsync(b => b.ProjectId == id);

        if (projectsEntities == null)
            return null!;

        var projects = new Project(projectsEntities.ProjectId, projectsEntities.Name,
                                   projectsEntities.Description, projectsEntities.TagProjectId)
            {
                Notes = projectsEntities.Note.Select(n => new Note(n.NoteId, n.Title, n.Description, n.Status,
                                                                   n.TagNoteId, n.DueDate, n.ProjectId)).ToList()
            };

        return projects;
    }

    public async Task<Guid> CreateProjects(Project projects)
    {
        var projectsEntities = new ProjectsEntity
        {
            ProjectId = projects.ProjectId,
            Name = projects.Name,
            Description = projects.Description,
            TagProjectId = projects.TagProjectId,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Projects.AddAsync(projectsEntities);
        await _context.SaveChangesAsync();
        return projectsEntities.ProjectId;
    }

    public async Task<Guid> UpdateProjects(Guid projectId, string name, string? description, int tagProjectId)
    {
        await _context.Projects
           .Where(b => b.ProjectId == projectId)
           .ExecuteUpdateAsync(s => s
               .SetProperty(b => b.Name, b => name)
               .SetProperty(b => b.Description, b => description)
               .SetProperty(b => b.TagProjectId, b => tagProjectId));

        return projectId;
    }

    public async Task<Guid> DeleteProjects(Guid id)
    {
        await _context.Projects
            .Where(b => b.ProjectId == id)
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<Project> SearchTitle(string name)
    {
        var projectsEntities = await _context.Projects
        .AsNoTracking()
        .FirstOrDefaultAsync(b => b.Name == name);

        if (projectsEntities == null)
            return null!;

        var projects = new Project(projectsEntities.ProjectId, projectsEntities.Name,
                                   projectsEntities.Description, projectsEntities.TagProjectId)
        {
            Notes = projectsEntities.Note.Select(n => new Note(n.NoteId, n.Title, n.Description, n.Status,
                                                               n.TagNoteId, n.DueDate, n.ProjectId)).ToList()
        };

        return projects;
    }
}
