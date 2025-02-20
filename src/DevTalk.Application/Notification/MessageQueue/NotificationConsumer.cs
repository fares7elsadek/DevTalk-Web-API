using DevTalk.API.Hubs;
using DevTalk.Application.Services.Notification;
using DevTalk.Domain.Entites;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace DevTalk.Application.Notification.MessageQueue;

public class NotificationConsumer(INotificationService notificationService,
    IHubContext<NotificationHub> hubContext) : IConsumer<NotificationMessage>
{
    public async Task Consume(ConsumeContext<NotificationMessage> context)
    {
        List<Notifications> notifications = new List<Notifications>();
        foreach(var userId in context.Message.UserIds)
        {
            notifications.Add(new Notifications
            {
                UserId = userId,
                Content = context.Message.Content,
                Timestamp = context.Message.Timestamp,
            });

            await hubContext.Clients.Group(userId)
                .SendAsync("ReceiveNotification", new {
                    UserId = userId,
                    context.Message.Content,
                    context.Message.Timestamp,
                });
        }
        await notificationService.SendNotificationsAsync(notifications);
    }
}
