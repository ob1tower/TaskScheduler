using CSharpFunctionalExtensions;
using NoteService.API.Dtos.Notes;
using NoteService.API.Dtos.Projects;
using NoteService.API.Enums;
using NoteService.API.Models;
using NoteService.API.Repositories.Interfaces;
using NoteService.API.Services.Interfaces;

namespace NoteService.API.Services;

public class ProjectsService : IProjectsService
{
    private readonly IProjectsRepository _projectsRepository;
    private readonly ILogger<ProjectsService> _logger;

    public ProjectsService(IProjectsRepository projectsRepository, ILogger<ProjectsService> logger)
    {
        _projectsRepository = projectsRepository;
        _logger = logger;
    }

    public async Task<Result<List<ProjectGetDto>>> GetAllProjects()
    {
        _logger.LogInformation("Запрос на получения списка проектов.");

        var projects = await _projectsRepository.GetProjects();

        var result = projects.Select(project => new ProjectGetDto(project.ProjectId, project.Name, project.Description,
                                                            project.TagProjectId, project.CreatedAt)).ToList();
        _logger.LogInformation("Получен список проектов.");

        return Result.Success(result);
    }

    public async Task<Result<ProjectWithNoteGetDto>> GetProject(Guid id)
    {
        _logger.LogInformation($"Запрос на получение задачи с Id: {id}");

        var projects = await _projectsRepository.GetProject(id);

        if (projects == null)
        {
            _logger.LogWarning($"Проект с Id {id} не найден.");
            return Result.Failure<ProjectWithNoteGetDto>($"Проект с Id {id} не найден.");
        }

        var result = new ProjectWithNoteGetDto(projects.ProjectId, projects.Name, projects.Description,
                                         projects.TagProjectId, projects.CreatedAt, projects.Notes.Select(p => new NoteGetDto(p.NoteId, p.Title, p.Description, p.Status,
                                                                                                                              p.TagNoteId, p.CreatedAt, p.DueDate)).ToList());
        return Result.Success(result);
    }

    public async Task<Result<Guid>> CreateProjects(ProjectCreateDto projectCreateDto)
    {
        _logger.LogInformation("Запрос на создание проект.");

        var name = await _projectsRepository.SearchTitle(projectCreateDto.Name);

        if (name != null)
        {
            _logger.LogWarning($"Проект с названием '{projectCreateDto.Name}' уже существует.");
            return Result.Failure<Guid>($"Проект с названием '{projectCreateDto.Name}' уже существует.");
        }

        var project = new Project(Guid.NewGuid(), projectCreateDto.Name,
                                  null, (int)ProjectTag.Inactive);

        var createProject = await _projectsRepository.CreateProjects(project);

        _logger.LogInformation($"Проект с Id {createProject} успешно создана с названием '{projectCreateDto.Name}'.");

        return Result.Success(createProject);
    }

    public async Task<Result<Guid>> UpdateProjects(Guid id, ProjectUpdateDto projectUpdateDto)
    {
        _logger.LogInformation($"Запрос на обновление проекта с Id: {id}");

        var title = await _projectsRepository.SearchTitle(projectUpdateDto.Name);
        if (title != null && title.ProjectId != id)
        {
            _logger.LogWarning($"Проект с названием '{projectUpdateDto.Name}' уже существует.");
            return Result.Failure<Guid>($"Проект с названием '{projectUpdateDto.Name}' уже существует.");
        }

        var project = new Project(id, projectUpdateDto.Name,
                                  projectUpdateDto.Description, projectUpdateDto.TagProjectId);


        var updatedProjectId = await _projectsRepository.UpdateProjects(id, projectUpdateDto.Name,
                                                        projectUpdateDto.Description, projectUpdateDto.TagProjectId);

        _logger.LogInformation($"Проект с Id {updatedProjectId} успешно обновлена.");

        return Result.Success(updatedProjectId);

    }

    public async Task<Result> DeleteProjects(Guid id)
    {
        _logger.LogInformation($"Запрос на удаление Id проекта: {id}");

        var projectId = await _projectsRepository.GetProject(id);

        if (projectId == null)
        {
            _logger.LogWarning($"Проект с Id {id} не найдена.");
            return Result.Failure($"Проект с Id {id} не найдена.");
        }

        var deletedProjectId = await _projectsRepository.DeleteProjects(id);

        _logger.LogInformation($"Проект с Id {deletedProjectId} успешно удалена.");

        return Result.Success(deletedProjectId);
    }
}
