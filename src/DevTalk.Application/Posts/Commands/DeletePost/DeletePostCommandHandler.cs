using DevTalk.Application.Services;
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
            IncludeProperties: "PostMedias");

        if (post is null)
            throw new NotFoundException(nameof(post), request.PostId);
        
        List<string> FilesNames = post.PostMedias.Select(x => x.MediaFileName).ToList();
        
        bool result = await unitOfWork.Post.DeletePostWithRelation(request.PostId);
        
        if (!result)
            throw new CustomeException("Something wrong has happened");
        
        await Task.WhenAll(FilesNames.Select(x => fileService.DeleteFile(x)));
        
        Task.Run(() => publisher.Publish(new DeletePostEvent(request.PostId), cancellationToken));
    }
}
