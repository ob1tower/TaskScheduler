using Microsoft.EntityFrameworkCore;
using NoteService.API.DataAccess;
using NoteService.API.Entities;
using NoteService.API.Enums;
using NoteService.API.Models;
using NoteService.API.Repositories.Interfaces;

namespace NoteService.API.Repositories;

public class NotesRepository : INotesRepository
{
    private readonly NotesDbContext _context;

    public NotesRepository(NotesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Note>> GetAllNotes()
    {
        var notesEntities = await _context.Notes
            .Include(n => n.Project)
            .AsNoTracking()
            .ToListAsync();

        var notes = notesEntities
            .Select(b => new Note(b.NoteId, b.Title, b.Description, b.Status,
                                  (int)b.TagNoteId, b.DueDate, b.ProjectId)).ToList();

        return notes;
    }

    public async Task<Note> GetNote(Guid id)
    {
        var notesEntities = await _context.Notes
            .Include(n => n.Project)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.NoteId == id);

        if (notesEntities == null)
            return null!;

        var notes = new Note(notesEntities.NoteId, notesEntities.Title, notesEntities.Description, notesEntities.Status,
                             notesEntities.TagNoteId, notesEntities.DueDate, notesEntities.ProjectId);

        return notes;
    }

    public async Task<Guid> CreateNotes(Note notes)
    {
        var notesEntities = new NotesEntity
        {
            NoteId = notes.NoteId,
            Title = notes.Title,
            Description = notes.Description,
            Status = notes.Status,
            TagNoteId = notes.TagNoteId,
            DueDate = notes.DueDate,
            ProjectId = notes.ProjectId,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.Notes.AddAsync(notesEntities);
        await _context.SaveChangesAsync();
        return notesEntities.NoteId;
    }

    public async Task<Guid> UpdateNotes(Guid noteId, string title, string? description, NoteStatus status, int tagNoteId, DateTime dueDate, Guid projectId)
    {
        await _context.Notes
            .Where(b => b.NoteId == noteId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.Title, b => title)
                .SetProperty(b => b.Description, b => description)
                .SetProperty(b => b.Status, b => status)
                .SetProperty(b => b.TagNoteId, b => tagNoteId)
                .SetProperty(b => b.DueDate, b => dueDate)
                .SetProperty(b => b.ProjectId, b => projectId));

        return noteId;
    }

    public async Task<Guid> DeleteNotes(Guid id)
    {
        await _context.Notes
            .Where(b => b.NoteId == id)
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<Guid?> SearchProjectId(Guid id)
    {
        var project = await _context.Projects
            .AsNoTracking() 
            .FirstOrDefaultAsync(p => p.ProjectId == id);

        if (project == null)
            return null!;

        return project.ProjectId;
    }

    public async Task<Note> SearchTitle(string title)
    {
        var notesEntities = await _context.Notes
        .AsNoTracking()
        .FirstOrDefaultAsync(note => note.Title == title);

        if (notesEntities == null)
            return null!;

        var notes = new Note(notesEntities.NoteId, notesEntities.Title, notesEntities.Description, notesEntities.Status,
                             notesEntities.TagNoteId, notesEntities.DueDate, notesEntities.ProjectId);

        return notes;
    }
}