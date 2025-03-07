using MediatR;
using Microsoft.AspNetCore.Http;

namespace DevTalk.Application.ApplicationUser.Commands.UploadAvatar;

public class UploadAvatarCommand:IRequest<string>
{
    public IFormFile Avatar { get; set; } = default!;
}
