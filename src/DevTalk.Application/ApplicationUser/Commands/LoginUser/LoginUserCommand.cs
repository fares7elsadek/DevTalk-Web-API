using DevTalk.Domain.Helpers;
using MediatR;

namespace DevTalk.Application.ApplicationUser.Commands.LoginUser;

public class LoginUserCommand:IRequest<AuthResponse>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
