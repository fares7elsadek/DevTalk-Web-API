using DevTalk.Application.Services;
using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Posts.Commands.DeletePost;

public class DeletePostCommandHandler(IUnitOfWork unitOfWork,
    IFileService fileService,IPublisher publisher) : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await unitOfWork.Post.GetOrDefalutAsync(
            d => d.PostId == request.PostId,
            IncludeProperties: "PostMedias,Votes,Comments,Bookmarks");

        if (post is null)
            throw new NotFoundException(nameof(post), request.PostId);

        await Task.WhenAll(post.PostMedias.Select(x => fileService.DeleteFile(x.MediaFileName)));

        unitOfWork.PostMedia.RemoveRange(post.PostMedias);
        unitOfWork.Comment.RemoveRange(post.Comments);
        unitOfWork.PostVotes.RemoveRange(post.Votes);
        unitOfWork.Bookmark.RemoveRange(post.Bookmarks);
        unitOfWork.Post.Remove(post);

        await unitOfWork.SaveAsync();
        await publisher.Publish(new DeletePostEvent(request.PostId), cancellationToken);
    }
}
