using CSharpFunctionalExtensions;
using FluentValidation;
using NoteService.API.Dtos.Notes;
using NoteService.API.Enums;
using NoteService.API.Models;
using NoteService.API.Repositories;
using NoteService.API.Repositories.Interfaces;
using NoteService.API.Services.Interfaces;

namespace NoteService.API.Services;

public class NotesService : INotesService
{
    private readonly INotesRepository _notesRepository;
    private readonly ILogger<NotesService> _logger;

    public NotesService(INotesRepository notesRepository, ILogger<NotesService> logger)
    {
        _notesRepository = notesRepository;
        _logger = logger;
    }

    public async Task<Result<List<NoteGetDto>>> GetAllNotes()
    {
        _logger.LogInformation("Запрос на получения списка задач.");

        var notes = await _notesRepository.GetAllNotes();

        var result = notes.Select(note => new NoteGetDto(note.NoteId, note.Title, note.Description, note.Status,
                                                         note.TagNoteId, note.DueDate, note.CreatedAt)).ToList();

        _logger.LogInformation("Получен список задач.");

        return Result.Success(result);
    }

    public async Task<Result<NoteGetDto>> GetNote(Guid id)
    {
        _logger.LogInformation($"Запрос на получение задачи с Id: {id}");

        var note = await _notesRepository.GetNote(id);

        if (note == null)
        {
            _logger.LogWarning($"Задача с Id {id} не найдена.");
            return Result.Failure<NoteGetDto>($"Задача с Id {id} не найдена.");
        }

        var result = new NoteGetDto(note.NoteId, note.Title, note.Description,
                                    note.Status, note.TagNoteId, note.DueDate, note.CreatedAt);

        return Result.Success(result);
    }

    public async Task<Result<Guid>> CreateNotes(NoteCreateDto noteCreateDto)
    {
        _logger.LogInformation("Запрос на создание задачи.");

        var projectId = await _notesRepository.SearchProjectId(noteCreateDto.ProjectId);

        if (projectId == null)
        {
            _logger.LogWarning($"Проект с Id {noteCreateDto.ProjectId} не найден.");
            return Result.Failure<Guid>($"Проект с Id {noteCreateDto.ProjectId} не найден.");
        }

        var title = await _notesRepository.SearchTitle(noteCreateDto.Title);

        if (title != null)
        {
            _logger.LogWarning($"Задача с заголовком '{noteCreateDto.Title}' уже существует.");
            return Result.Failure<Guid>($"Задача с заголовком '{noteCreateDto.Title}' уже существует.");
        }

        var note = new Note(Guid.NewGuid(), noteCreateDto.Title, null, NoteStatus.Pending,
                            (int)NoteTag.Low, DateTime.UtcNow, noteCreateDto.ProjectId);

        var createNote = await _notesRepository.CreateNotes(note);

        _logger.LogInformation($"Задача с Id {createNote} успешно создана с заголовком '{noteCreateDto.Title}'.");

        return Result.Success(createNote); 
    }

    public async Task<Result<Guid>> UpdateNotes(Guid id, NoteUpdateDto noteUpdateDto)
    {
        _logger.LogInformation($"Запрос на обновление задачи с Id: {id}");

        var projectId = await _notesRepository.SearchProjectId(noteUpdateDto.ProjectId);
        if (projectId == null)
        {
            _logger.LogWarning($"Проект с Id {noteUpdateDto.ProjectId} не найден.");
            return Result.Failure<Guid>($"Проект с Id {noteUpdateDto.ProjectId} не найден.");
        }

        var title = await _notesRepository.SearchTitle(noteUpdateDto.Title);
        if (title != null && title.NoteId != id)
        {
            _logger.LogWarning($"Задача с заголовком '{noteUpdateDto.Title}' уже существует.");
            return Result.Failure<Guid>($"Задача с заголовком '{noteUpdateDto.Title}' уже существует.");
        }

        var note = new Note(id, noteUpdateDto.Title, noteUpdateDto.Description, noteUpdateDto.Status,
                            noteUpdateDto.TagNoteId, noteUpdateDto.DueDate, noteUpdateDto.ProjectId);

        var updatedNoteId = await _notesRepository.UpdateNotes(id, noteUpdateDto.Title, noteUpdateDto.Description, noteUpdateDto.Status,
                                                               noteUpdateDto.TagNoteId, noteUpdateDto.DueDate, noteUpdateDto.ProjectId);

        _logger.LogInformation($"Задача с Id {updatedNoteId} успешно обновлена.");

        return Result.Success(updatedNoteId);
    }

    public async Task<Result> DeleteNotes(Guid id)
    {
        _logger.LogInformation($"Запрос на удаление Id задачи: {id}");

        var noteId = await _notesRepository.GetNote(id);

        if (noteId == null)
        {
            _logger.LogWarning($"Задача с Id {id} не найдена.");
            return Result.Failure($"Задача с Id {id} не найдена.");
        }

        var deletedNoteId = await _notesRepository.DeleteNotes(id);

        _logger.LogInformation($"Задача с Id {deletedNoteId} успешно удалена.");

        return Result.Success(deletedNoteId);
    }
}