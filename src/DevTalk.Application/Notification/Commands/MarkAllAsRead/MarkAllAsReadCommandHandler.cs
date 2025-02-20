using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Services.Notification;
using MediatR;

namespace DevTalk.Application.Notification.Commands.MarkAllAsRead;

public class MarkAllAsReadCommandHandler(IUserContext userContext,
    INotificationService notificationService) : IRequestHandler<MarkAllAsReadCommand>
{
    public async Task Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        await notificationService.MarkAllAsReadAsync(userContext.
            GetCurrentUser().userId);
    }
}
