using MediatR;

namespace DevTalk.Application.Notification.Commands.MarkAsRead;

public class MarkAsReadCommand(string notificationId):IRequest
{
    public string NotificationId { get; set; } = notificationId;
}
