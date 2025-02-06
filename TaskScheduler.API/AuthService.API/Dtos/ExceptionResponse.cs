using System.Net;

namespace AuthService.API.Dtos;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
