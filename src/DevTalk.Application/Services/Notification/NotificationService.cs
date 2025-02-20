using AutoMapper;
using DevTalk.Application.Notification.Dtos;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;

namespace DevTalk.Application.Services.Notification;

public class NotificationService(IUnitOfWork unitOfWork,
    IMapper mapper) : INotificationService
{
    public async Task<IEnumerable<NotificationDto>> GetUnReadNotificaitons(string userId)
    {
        var notifications = await unitOfWork.Notification.GetAllWithConditionAsync(
            x => x.UserId == userId && x.IsRead == false);
        var result = mapper.Map<IEnumerable<NotificationDto>>(notifications);
        return result;
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        var notifications = await unitOfWork.Notification.GetAllWithConditionAsync(
            x => x.UserId == userId);
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }
        await unitOfWork.SaveAsync();
    }

    public async Task MarkAsReadAsync(string notificationId,string userId)
    {
        var notification = await unitOfWork.Notification.GetOrDefalutAsync(
            x => x.Id == notificationId);
        if(notification!= null && notification.UserId == userId)
        {
            notification!.IsRead = true;
            await unitOfWork.SaveAsync();
        }
        else
        {
            throw new CustomeException("User not authorized");
        }
    }

    public async Task SendNotificationsAsync(List<Notifications> notifications)
    {
        await unitOfWork.Notification.AddRange(notifications);
        await unitOfWork.SaveAsync();
    }
}
