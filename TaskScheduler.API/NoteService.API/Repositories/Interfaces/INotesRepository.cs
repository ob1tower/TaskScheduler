using NoteService.API.Enums;
using NoteService.API.Models;

namespace NoteService.API.Repositories.Interfaces
{
    public interface INotesRepository
    {
        Task<Guid> CreateNotes(Note notes);
        Task<Guid> DeleteNotes(Guid id);
        Task<List<Note>> GetAllNotes();
        Task<Note> GetNote(Guid id);
        Task<Guid> UpdateNotes(Guid noteId, string title, string? description, NoteStatus status, int tagNoteId, DateTime dueDate, Guid projectId);
        Task<Guid?> SearchProjectId(Guid id);
        Task<Note> SearchTitle(string title);
    }
}