using DevTalk.Application.Services;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.ApplicationUser.Commands.UploadAvatar;

public class UploadAvatarCommandHandler(IUserContext userContext,
    IFileService fileService,IUnitOfWork unitOfWork) : IRequestHandler<UploadAvatarCommand, string>
{
    public async Task<string> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var userId = userContext.GetCurrentUser().userId;
        if (userId == null)
            throw new CustomeException("Something worng has happened");
        var user = await unitOfWork.User.GetOrDefalutAsync(x => x.Id == userId);
        if (user == null)
            throw new CustomeException("Something worng has happened");

        if(!string.IsNullOrEmpty(user.AvatarFileName))
            await fileService.DeleteFile(user.AvatarFileName);

        var maxFileSize = 5 * 1024 * 1024;
        if (request.Avatar.Length > maxFileSize)
            throw new CustomeException("The maximum file size is 5 MB");

        string[] allowedImageExtensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"
        };

        var (Url, fileName) = await fileService.SaveFileAsync(request.Avatar, allowedImageExtensions);
        user.AvatarFileName = fileName;
        user.AvatarUrl = Url;
        await unitOfWork.SaveAsync();

        return Url;
    }
}
