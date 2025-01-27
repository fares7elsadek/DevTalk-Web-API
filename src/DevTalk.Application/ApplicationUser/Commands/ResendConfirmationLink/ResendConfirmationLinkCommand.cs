using MediatR;

namespace DevTalk.Application.ApplicationUser.Commands.ResendConfirmationLink;

public class ResendConfirmationLinkCommand:IRequest<string>
{
    public string Email { get; set; } = default!;
}
