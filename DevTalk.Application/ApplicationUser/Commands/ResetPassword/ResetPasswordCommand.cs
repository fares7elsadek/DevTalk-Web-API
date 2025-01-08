using MediatR;
using System.Text.Json.Serialization;

namespace DevTalk.Application.ApplicationUser.Commands.ResetPassword;

public class ResetPasswordCommand:IRequest<string>
{
    [JsonIgnore]
    public string? UserId { get; set; }
    [JsonIgnore]
    public string? Token { get; set; }
    public string NewPassword { get; set; } = default!;
}
