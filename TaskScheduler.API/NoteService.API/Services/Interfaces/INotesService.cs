using CSharpFunctionalExtensions;
using NoteService.API.Dtos.Notes;

namespace NoteService.API.Services.Interfaces
{
    public interface INotesService
    {
        Task<Result<Guid>> CreateNotes(NoteCreateDto noteCreateDto);
        Task<Result> DeleteNotes(Guid id);
        Task<Result<List<NoteGetDto>>> GetAllNotes();
        Task<Result<NoteGetDto>> GetNote(Guid id);
        Task<Result<Guid>> UpdateNotes(Guid id, NoteUpdateDto noteUpdateDto);
    }
}