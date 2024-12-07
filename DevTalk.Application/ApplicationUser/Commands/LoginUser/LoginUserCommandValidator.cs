using FluentValidation;

namespace DevTalk.Application.ApplicationUser.Commands.LoginUser;

public class LoginUserCommandValidator:AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(d => d.Email)
            .EmailAddress().WithMessage("Email address is not valid.");
    }
}
