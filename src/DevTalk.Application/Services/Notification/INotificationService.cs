using DevTalk.Application.Notification.Dtos;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Services.Notification;

public interface INotificationService
{
    public Task SendNotificationsAsync(List<Notifications> notifications);
    public Task MarkAsReadAsync(string notificationId,string userId);
    public Task MarkAllAsReadAsync(string userId);
    public Task<IEnumerable<NotificationDto>> GetUnReadNotificaitons(string userId);
}
