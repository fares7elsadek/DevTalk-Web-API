namespace DevTalk.Domain.Helpers;

public class JwtOptions
{
    public string Key { get; set; } = default!;
    public string Issure { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int DurationInMinutes { get; set; }
}
