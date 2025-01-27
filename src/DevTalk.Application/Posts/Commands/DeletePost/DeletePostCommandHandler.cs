using DevTalk.Application.Services;
using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Posts.Commands.DeletePost;

public class DeletePostCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext,IFileService fileService) : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var id = request.PostId;
        if(id == null)
            throw new ArgumentNullException("id");
        var user = userContext.GetCurrentUser();
        var post = await unitOfWork.Post.GetOrDefalutAsync(d => d.PostId == id,
            IncludeProperties: "PostMedias,Votes,Comments,User");
        if(post == null)
            throw new NotFoundException(nameof(post),id);

        if (!user.IsInRole(UserRoles.Admin))
        {
            var PostUserId = post.User.Id;
            if (user.userId != PostUserId)
                throw new CustomeException("User not authroized");
        }

        var postMediasPath = post.PostMedias.Select(x => x.MediaPath).ToList();
        foreach(var path in postMediasPath)
        {
            fileService.DeleteFile(path);
        }
        unitOfWork.PostMedia.RemoveRange(post.PostMedias);
        unitOfWork.Comment.RemoveRange(post.Comments);
        unitOfWork.PostVotes.RemoveRange(post.Votes);
        unitOfWork.Post.Remove(post);
        await unitOfWork.SaveAsync();
    }
}
