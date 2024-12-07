using DevTalk.Domain.Helpers;
using MediatR;

namespace DevTalk.Application.ApplicationUser.Commands.RegisterUser;

public class RegisterUserCommand:IRequest<AuthResponse>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Email {  get; set; } = default!;
    public string Password { get; set; } = default!;
}
