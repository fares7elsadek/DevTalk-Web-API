using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Services.Notification;
using Microsoft.AspNetCore.SignalR;

namespace DevTalk.API.Hubs;

public class NotificationHub(IUserContext userContext,
    INotificationService notificationService):Hub
{

    public async override Task OnConnectedAsync()
    {
        var userId = userContext.GetCurrentUser().userId;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            var notifications = await notificationService.GetUnReadNotificaitons(userId);
            if (notifications.Any())
            {
                await Clients.Caller.SendAsync("ReceiveNotifications", notifications);
            }
        }
        await base.OnConnectedAsync();
    }
}
