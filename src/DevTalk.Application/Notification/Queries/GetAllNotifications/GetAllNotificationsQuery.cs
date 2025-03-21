using DevTalk.Application.Notification.Dtos;
using MediatR;

namespace DevTalk.Application.Notification.Queries.GetAllNotifications;

public record GetAllNotificationsQuery(string cursor,int pageSize):IRequest<GetUserNotificationsDto>
{
}
