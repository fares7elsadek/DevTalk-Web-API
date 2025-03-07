using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Posts.Commands.UpdatePosts;

public class UpdatePostsCommandHandler(IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<UpdatePostsCommand>
{
    public async Task Handle(UpdatePostsCommand request, CancellationToken cancellationToken)
    {
        if (request.PostId == null)
            throw new ArgumentNullException(nameof(request.PostId), "Post ID cannot be null");

        if (string.IsNullOrWhiteSpace(request.Title) && string.IsNullOrWhiteSpace(request.Body))
            throw new CustomeException("You must update at least one field");

        var post = await unitOfWork.Post
            .GetOrDefalutAsync(d => d.PostId == request.PostId);

        if (post == null)
            throw new NotFoundException(nameof(post), request.PostId);

        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(request.Title) && post.Title != request.Title)
        {
            post.Title = request.Title;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(request.Body) && post.Body != request.Body)
        {
            post.Body = request.Body;
            isUpdated = true;
        }

        if (!isUpdated) return; 

        await unitOfWork.SaveAsync(); 

        await publisher.Publish(new UpdatePostEvent
        {
            Title = request.Title,
            Body = request.Body,
            PostId = request.PostId
        }, cancellationToken);
    }
}
