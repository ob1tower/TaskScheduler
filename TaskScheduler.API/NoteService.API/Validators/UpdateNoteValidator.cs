using FluentValidation;
using NoteService.API.Dtos.Notes;

namespace NoteService.API.Validators
{
    public class UpdateNoteValidator : AbstractValidator<NoteUpdateDto>
    {
        public UpdateNoteValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(250).WithMessage("Заголовок не может быть длиннее 250 символов.");

            RuleFor(x => x.Description)
                .MaximumLength(5000).WithMessage("Описание не может быть длиннее 5000 символов.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Status)
                .NotEmpty()
                .IsInEnum().WithMessage("Неверный Статус. Допустимые значения: 1 (Pending), 2 (InProgress) и 3 (Completed).");

            RuleFor(x => x.TagNoteId)
                .NotEmpty()
                .Must(id => new[] { 1, 2, 3 }.Contains(id))
                .WithMessage("Неверный TagNoteId. Допустимые значения: 1 (Low), 2 (Medium) и 3 (High).");

            RuleFor(x => x.DueDate.Date)
                .NotEmpty()
                .Must(dueDate => dueDate.Date >= DateTime.UtcNow.Date).WithMessage("Срок выполнения не может быть в прошлом.");
        }
    }
}
