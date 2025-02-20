namespace DevTalk.Application.Notification.Dtos;

public class NotificationDto
{
    public string UserId { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTime Timestamp { get; set; }
}
