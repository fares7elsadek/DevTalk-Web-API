using AutoMapper;
using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Notification.Dtos;
using DevTalk.Application.Services.Notification;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Notification.Queries.GetAllNotifications;

public class GetAllNotificationsQueryHandler(IUserContext userContext
    ,IUnitOfWork unitOfWork,IMapper mapper) : IRequestHandler<GetAllNotificationsQuery,
    IEnumerable<NotificationDto>>
{
    public async Task<IEnumerable<NotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var userId = userContext.GetCurrentUser().userId;
        if (userId == null)
            throw new CustomeException("Something wrong has happened");

        var notifications = await unitOfWork.Notification.GetAllWithConditionAsync(x =>
        x.UserId == userId);
        var result = mapper.Map<IEnumerable<NotificationDto>>(notifications);
        return result.OrderByDescending(x => x.Timestamp);
    }
}
