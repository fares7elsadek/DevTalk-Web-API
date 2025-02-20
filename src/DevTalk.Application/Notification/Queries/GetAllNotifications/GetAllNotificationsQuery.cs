using DevTalk.Application.Notification.Dtos;
using MediatR;

namespace DevTalk.Application.Notification.Queries.GetAllNotifications;

public class GetAllNotificationsQuery:IRequest<IEnumerable<NotificationDto>>
{
}
