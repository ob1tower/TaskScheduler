using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NoteService.API.DataAccess;
using NoteService.API.Dtos.Notes;
using NoteService.API.Repositories;
using NoteService.API.Repositories.Interfaces;
using NoteService.API.Services;
using NoteService.API.Services.Interfaces;
using NoteService.API.Validators;

namespace NoteService.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NotesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("NotesDbContext")));

        services.AddScoped<INotesRepository, NotesRepository>();
        services.AddScoped<IProjectsRepository, ProjectsRepository>();

        services.AddScoped<INotesService, NotesService>();
        services.AddScoped<IProjectsService, ProjectsService>();

        services.AddTransient<IValidator<NoteUpdateDto>, UpdateNoteValidator>();
        services.AddTransient<IValidator<NoteCreateDto>, CreateNoteValidator>();

        services.AddValidatorsFromAssemblyContaining<CreateNoteValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateNoteValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateProjectValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateProjectValidator>();

        return services;
    }
}
