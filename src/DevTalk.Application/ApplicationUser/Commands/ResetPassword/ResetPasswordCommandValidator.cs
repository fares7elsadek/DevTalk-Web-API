using FluentValidation;

namespace DevTalk.Application.ApplicationUser.Commands.ResetPassword;

public class ResetPasswordCommandValidator:AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(p => p.NewPassword)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(16).WithMessage("Password must not exceed 16 characters.")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one numeric digit.")
            .Matches(@"[\!\?\*\.\@\#\$]+").WithMessage("Password must contain at least one special character (!, ?, *, @ , # or $).");
    }
}
