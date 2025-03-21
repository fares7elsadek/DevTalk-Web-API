using AutoMapper;
using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Notification.Dtos;
using DevTalk.Application.Posts;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Notification.Queries.GetAllNotifications;

public class GetAllNotificationsQueryHandler(IUserContext userContext
    ,IUnitOfWork unitOfWork,IMapper mapper) : IRequestHandler<GetAllNotificationsQuery,
    GetUserNotificationsDto>
{
    public async Task<GetUserNotificationsDto> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var userId = userContext.GetCurrentUser().userId;
        
        var decodedTime = DateTimeCursorOperations.Decode(request.cursor)!;
        var notifications = await unitOfWork.Notification
            .GetUserNotifications(userId,decodedTime,request.pageSize);
        
        if (notifications.Any())
        {
            var lastNotification = notifications.LastOrDefault()!;
            string cursor = DateTimeCursorOperations.Encode(lastNotification.Timestamp);
            var notificationsDto = mapper.Map<IEnumerable<NotificationDto>>(notifications).ToList();
            var result  = new GetUserNotificationsDto(cursor, notificationsDto);
            return result;
        }

        return new GetUserNotificationsDto("", null);
    }
}
