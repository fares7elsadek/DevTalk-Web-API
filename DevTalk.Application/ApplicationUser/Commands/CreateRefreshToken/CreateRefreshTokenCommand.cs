using DevTalk.Domain.Helpers;
using MediatR;

namespace DevTalk.Application.ApplicationUser.Commands.CreateRefreshToken;

public class CreateRefreshTokenCommand(string token):IRequest<AuthResponse>
{
    public string Token { get; set; } = token;
}
