using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Services.Notification;
using MediatR;

namespace DevTalk.Application.Notification.Commands.MarkAsRead;

public class MarkAsReadCommandHandler(IUserContext userContext,
    INotificationService notificationService) : IRequestHandler<MarkAsReadCommand>
{
    public async Task Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        await notificationService.MarkAsReadAsync(request.NotificationId,userContext
            .GetCurrentUser().userId);
    }
}
