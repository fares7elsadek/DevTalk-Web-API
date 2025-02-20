namespace DevTalk.Application.Notification.MessageQueue;

public record NotificationMessage
{
    public NotificationMessage()
    {
        UserIds = new HashSet<string>();
    }
    public ICollection<string> UserIds { get; set; }
    public string Content { get; set; } = default!;
    public DateTime Timestamp { get; set; }
}
