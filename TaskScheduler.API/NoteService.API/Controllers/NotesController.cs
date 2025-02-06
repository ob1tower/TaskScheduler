using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteService.API.Dtos.Notes;
using NoteService.API.Error;
using NoteService.API.Services.Interfaces;
using NoteService.API.Types;

namespace NoteService.API.Controllers;

[Authorize(AuthenticationSchemes = "Access",Roles = $"{RolesType.User}")]
[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;
    private readonly IValidator<NoteCreateDto> _createNoteValidator;
    private readonly IValidator<NoteUpdateDto> _updateNoteValidator;

    public NotesController(INotesService notesService, IValidator<NoteCreateDto> createNoteValidator, IValidator<NoteUpdateDto> updateNoteValidator)
    {
        _notesService = notesService;
        _createNoteValidator = createNoteValidator;
        _updateNoteValidator = updateNoteValidator;
    }

    /// <summary>
    /// Получение списка всех задач
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllNotes()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var result = await _notesService.GetAllNotes();

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = "Список задач.", notes = result.Value });
    }

    /// <summary>
    /// Создание новой задачи
    /// </summary>
    /// <param name="noteCreateDto">Данные для создания задачи</param>
    /// <returns></returns>
    [HttpPost("Create")]
    public async Task<IActionResult> CreateNote([FromBody] NoteCreateDto noteCreateDto)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var validationResult = await _createNoteValidator.ValidateAsync(noteCreateDto);

        if (!validationResult.IsValid)
            return BadRequest(ErrorFormat.Deserialize(validationResult.Errors));

        var result = await _notesService.CreateNotes(noteCreateDto);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = $"Задача с Id {result.Value} успешно создана." });
    }

    /// <summary>
    /// Обновление задачи
    /// </summary>
    /// <param name="id">ID задачи для обновления</param>
    /// <param name="noteUpdateDto">Данные для обновления задачи</param>
    /// <returns></returns>
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateNote(Guid id, [FromBody] NoteUpdateDto noteUpdateDto)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var validationResult = await _updateNoteValidator.ValidateAsync(noteUpdateDto);

        if (!validationResult.IsValid)
            return BadRequest(ErrorFormat.Deserialize(validationResult.Errors));

        var result = await _notesService.UpdateNotes(id, noteUpdateDto);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = $"Задача с Id {id} успешно обновлена." });
    }

    /// <summary>
    /// Удаление задачи по ID
    /// </summary>
    /// <param name="id">ID задачи для удаления</param>
    /// <returns></returns>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var result = await _notesService.DeleteNotes(id);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = $"Задача с Id {id} успешно удалена." });
    }

    /// <summary>
    /// Получение задачи по ID
    /// </summary>
    /// <param name="id">ID задачи</param>
    /// <returns></returns>
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetNote(Guid id)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var result = await _notesService.GetNote(id);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(new { message = $"Задача с Id {id} найдена.", note = result.Value });
    }
}