using MediatR;

namespace DevTalk.Application.ApplicationUser.Commands.ForgotPassword;

public class ForgotPasswordCommand:IRequest<string>
{
    public string Email { get; set; } = default!;
}
