using DevTalk.Application.ApplicationUser;
using DevTalk.Application.PostVote.Commands.CreatePostVoteDownVote;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.PostVote.Commands.CreatePostVote;

public class CreatePostVoteCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext) : IRequestHandler<CreatePostDownVoteCommand>
{
    public async Task Handle(CreatePostDownVoteCommand request, CancellationToken cancellationToken)
    {
        if (request.PostId == null)
            throw new ArgumentNullException(nameof(request.PostId));
        var post = await unitOfWork.Post.GetOrDefalutAsync(p => p.PostId == request.PostId,
            IncludeProperties: "Votes");
        if (post == null)
            throw new NotFoundException(nameof(post), request.PostId);

        var user = userContext.GetCurrentUser();
        if (user == null)
            throw new CustomeException("Not authenticated");

        var PostVote = post.Votes.FirstOrDefault(p => p.UserId == user.userId);
        if (PostVote == null)
        {
            PostVotes newPostvote = new PostVotes
            {
                UserId = user.userId,
                PostId = request.PostId,
                VoteType = VoteType.DownVote,
            };
            post.Votes.Add(newPostvote);
            await unitOfWork.SaveAsync();
        }
        else
        {
            if (PostVote.VoteType == VoteType.DownVote)
            {
                unitOfWork.PostVotes.Remove(PostVote);
                await unitOfWork.SaveAsync();
            }
            else
            {
                PostVote.VoteType = VoteType.DownVote;
                await unitOfWork.SaveAsync();
            }
        }
    }
}
