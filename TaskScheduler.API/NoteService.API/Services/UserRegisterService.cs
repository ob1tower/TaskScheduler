using NoteService.API.Dtos.Notes;
using NoteService.API.Dtos.Projects;
using NoteService.API.Services.Interfaces;

namespace NoteService.API.Services
{
    public class UserRegisterService
    {
        private readonly ILogger<UserRegisterService> _logger;
        private readonly IProjectsService _projectsService;
        private readonly INotesService _notesService;

        public UserRegisterService(ILogger<UserRegisterService> logger, IProjectsService projectsService,
               INotesService notesService)
        {
            _logger = logger;
            _projectsService = projectsService;
            _notesService = notesService;
        }

        public class EventMessage
        {
            public string? EventType { get; set; }
            public string? UserId { get; set; }
        }

        public async Task ProcessMessageAsync(string message)
        {
            var eventData = System.Text.Json.JsonSerializer.Deserialize<EventMessage>(message);

            if (eventData == null)
            {
                _logger.LogWarning("Сообщение пустое или имеет неправильный формат.");
                return;
            }

            _logger.LogInformation($"Получено сообщение с типом события: {eventData.EventType}.");

            switch (eventData.EventType)
            {
                case "register":
                    if (Guid.TryParse(eventData.UserId, out Guid userId))
                    {
                        _logger.LogInformation($"UserId пользователя: {userId}.");

                        var projectCreateDto = new ProjectCreateDto($"Проект_{userId}");
                        var projectResult = await _projectsService.CreateProjects(projectCreateDto);

                        if (projectResult.IsFailure)
                        {
                            _logger.LogWarning($"Ошибка при создании проекта для пользователя {userId}: {projectResult.Error}");
                            return;
                        }

                        _logger.LogInformation($"Проект успешно создан для пользователя {userId}.");

                        var noteCreateDto = new NoteCreateDto($"Задача_{userId}", projectResult.Value);
                        var noteResult = await _notesService.CreateNotes(noteCreateDto);

                        if (noteResult.IsFailure)
                        {
                            _logger.LogWarning($"Ошибка при создании задачи для проекта с Id {projectResult.Value}: {noteResult.Error}");
                            return;
                        }

                        _logger.LogInformation($"Задача успешно создана для проекта с Id {projectResult.Value}.");
                    }
                    break;

                default:
                    _logger.LogWarning($"Неизвестный тип события: {eventData.EventType}.");
                    break;
            }
        }
    }
}
