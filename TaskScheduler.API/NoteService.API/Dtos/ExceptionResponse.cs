using System.Net;

namespace NoteService.API.Dtos;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);