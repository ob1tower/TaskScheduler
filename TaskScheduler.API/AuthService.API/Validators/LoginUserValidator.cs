using AuthService.API.Dtos;
using AuthService.API.Models;
using FluentValidation;

namespace AuthService.API.Validators;

public class LoginUserValidator : AbstractValidator<UserLoginDto>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("Некорректный формат email.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(5).WithMessage("Пароль должен быть не менее 5 символов.");
    }
}
