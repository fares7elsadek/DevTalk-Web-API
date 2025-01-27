using System.Text.Json.Serialization;

namespace DevTalk.Domain.Helpers;

public class AuthResponse
{
    public AuthResponse()
    {
        Roles = new();
    }
    public string Message { get; set; } = default!;
    public bool IsAuthenticated { get; set; } 
    public string Username { get; set; } = default!;
    public string Email     { get; set; } = default!;
    public List<string> Roles { get; set; }
    public string Token { get; set; } = default!;

    [JsonIgnore]
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }

}
