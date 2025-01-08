using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevTalk.Application.ApplicationUser.Commands.ForgotPassword;

public class ForgotPasswordCommandValidator:AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(d => d.Email)
            .EmailAddress().WithMessage("Email address is not valid.");
    }
}
