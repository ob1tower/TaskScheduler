using FluentValidation;
using NoteService.API.Dtos.Notes;

namespace NoteService.API.Validators;

public class CreateNoteValidator : AbstractValidator<NoteCreateDto>
{
    public CreateNoteValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(250).WithMessage("Заголовок не может быть длиннее 250 символов.");
    }
}
