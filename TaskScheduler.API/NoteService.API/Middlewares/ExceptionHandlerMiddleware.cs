using FluentValidation;
using NoteService.API.Dtos;
using System.Net;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Произошла непредвиденная ошибка.");      

        ExceptionResponse response = exception switch
        {
            ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Произошло исключение из приложения."),
            KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "Ресурс не найден."),
            UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Неавторизованный."),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Внутренняя ошибка сервера. Пожалуйста, повторите попытку позже.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}