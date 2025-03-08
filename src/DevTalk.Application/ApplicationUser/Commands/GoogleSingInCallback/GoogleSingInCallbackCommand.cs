using DevTalk.Domain.Helpers;
using MediatR;

namespace DevTalk.Application.ApplicationUser.Commands.GoogleSingInCallback;

public class GoogleSingInCallbackCommand(string remoteError):IRequest<AuthResponse>
{
    public string RemoteError { get; set; } = remoteError;
}
