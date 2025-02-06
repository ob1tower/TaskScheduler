using AuthService.API.Dtos;
using FluentValidation;

namespace AuthService.API.Validators;

public class RegisterUserValidator : AbstractValidator<UserRegisterDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Имя пользователя не может быть пустым.")
            .MinimumLength(5).WithMessage("Имя пользователя должно быть не менее 5 символов.")
            .MaximumLength(20).WithMessage("Имя пользователя не может быть длиннее 50 символов.")
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Имя пользователя может содержать только буквы и цифры.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email не может быть пустым.")
            .EmailAddress().WithMessage("Некорректный формат email.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль не может быть пустым.")
            .MinimumLength(5).WithMessage("Пароль должен быть не менее 5 символов.");
    }
}
