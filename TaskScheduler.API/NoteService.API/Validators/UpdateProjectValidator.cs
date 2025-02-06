using FluentValidation;
using NoteService.API.Dtos.Projects;

namespace NoteService.API.Validators;

public class UpdateProjectValidator : AbstractValidator<ProjectUpdateDto>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(250).WithMessage("Название не может быть длиннее 250 символов.");

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Описание не может быть длиннее 5000 символов.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.TagProjectId)
            .NotEmpty()
            .Must(id => new[] { 1, 2, 3, 4 }.Contains(id))
            .WithMessage("Неверный TagProjectId. Допустимые значения: 1 (Inactive), 2 (InProgress), 3 (Suspended) и 4 (Completed).");
    }
}
