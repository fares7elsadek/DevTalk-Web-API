using AutoMapper;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Notification.Dtos;

public class NotificationProfile:Profile
{
    public NotificationProfile()
    {
        CreateMap<Notifications, NotificationDto>()
            .ReverseMap();
    }
}
