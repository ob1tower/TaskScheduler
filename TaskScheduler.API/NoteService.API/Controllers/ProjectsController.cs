using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteService.API.Dtos.Projects;
using NoteService.API.Error;
using NoteService.API.Services.Interfaces;
using NoteService.API.Types;

namespace NoteService.API.Controllers;

[Authorize(AuthenticationSchemes = "Access", Roles = $"{RolesType.User}")]
[ApiController]
[Route("[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectsService _projectsService;
    private readonly IValidator<ProjectCreateDto> _createProjectValidator;
    private readonly IValidator<ProjectUpdateDto> _updateProjectValidator;

    public ProjectsController(IProjectsService projectsService, IValidator<ProjectCreateDto> createProjectValidator, IValidator<ProjectUpdateDto> updateProjectValidator)
    {
        _projectsService = projectsService;
        _createProjectValidator = createProjectValidator;
        _updateProjectValidator = updateProjectValidator;
    }

    /// <summary>
    /// Получение списка всех проектов
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllProject()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var result = await _projectsService.GetAllProjects();

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = "Список проектов.", projects = result.Value });
    }

    /// <summary>
    /// Создание нового проекта
    /// </summary>
    /// <param name="projectCreateDto">Данные для создания проекта</param>
    /// <returns></returns>
    [HttpPost("Create")]
    public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto projectCreateDto)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var validationResult = await _createProjectValidator.ValidateAsync(projectCreateDto);

        if (!validationResult.IsValid)
            return BadRequest(ErrorFormat.Deserialize(validationResult.Errors));

        var result = await _projectsService.CreateProjects(projectCreateDto);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = $"Проект с Id {result.Value} успешно создана." });
    }

    /// <summary>
    /// Обновление проекта
    /// </summary>
    /// <param name="id">ID проекта для обновления</param>
    /// <param name="projectUpdateDto">Данные для обновления проекта</param>
    /// <returns></returns>
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectUpdateDto projectUpdateDto)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var validationResult = await _updateProjectValidator.ValidateAsync(projectUpdateDto);

        if (!validationResult.IsValid)
            return BadRequest(ErrorFormat.Deserialize(validationResult.Errors));

        var result = await _projectsService.UpdateProjects(id, projectUpdateDto);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = $"Проект с Id {id} успешно обновлена." });
    }

    /// <summary>
    /// Удаление проекта по ID
    /// </summary>
    /// <param name="id">ID проекта для удаления</param>
    /// <returns></returns>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var result = await _projectsService.DeleteProjects(id);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = $"Проект с Id {id} успешно удалена." });
    }

    /// <summary>
    /// Получение проекта по ID
    /// </summary>
    /// <param name="id">ID проекта</param>
    /// <returns></returns>
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetProject(Guid id)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var result = await _projectsService.GetProject(id);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = $"Проект с Id {id} найден.", project = result.Value });
    }
}
