namespace DevTalk.Domain.Entites;

public class Preference
{
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
    public string CategoryId { get; set; } = default!;
    public Categories Category { get; set; } = default!;
}
