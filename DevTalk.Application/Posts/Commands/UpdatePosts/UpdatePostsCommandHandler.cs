using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Posts.Commands.UpdatePosts;

public class UpdatePostsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePostsCommand>
{
    public async Task Handle(UpdatePostsCommand request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Title) && string.IsNullOrWhiteSpace(request.Body)) 
            throw new CustomeException("You must update at least one field");
        var Post = await unitOfWork.Post.GetOrDefalutAsync(p => p.PostId == request.PostId);
        if(Post == null)
            throw new NotFoundException(nameof(Post),request.PostId);
        if(!string.IsNullOrWhiteSpace(request.Title))
            Post.Title = request.Title;
        if(!string.IsNullOrWhiteSpace(request.Body))
            Post.Body = request.Body;
        unitOfWork.Post.Update(Post);
        await unitOfWork.SaveAsync();
    }
}
