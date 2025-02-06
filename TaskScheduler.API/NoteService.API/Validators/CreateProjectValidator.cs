using FluentValidation;
using NoteService.API.Dtos.Projects;

namespace NoteService.API.Validators;

public class CreateProjectValidator : AbstractValidator<ProjectCreateDto>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(250).WithMessage("Название не может быть длиннее 250 символов.");
    }
}
