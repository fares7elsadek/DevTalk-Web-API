namespace DevTalk.Domain.Entites;

public class Notifications
{
    public string Id { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; } = false;
}
