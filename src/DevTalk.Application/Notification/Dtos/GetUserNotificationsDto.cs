namespace DevTalk.Application.Notification.Dtos;

public record GetUserNotificationsDto(string cursor, IEnumerable<NotificationDto> notifications);
