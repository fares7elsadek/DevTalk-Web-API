using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetFeedPosts;

public class GetFeedPostsQueryHandler(IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetFeedPostsQuery,
    IEnumerable<PostDto>>
{
    public async Task<IEnumerable<PostDto>> Handle(GetFeedPostsQuery request, CancellationToken cancellationToken)
    {
        var prefernces = await unitOfWork.Preference
            .GetAllWithConditionAsync(x => x.UserId == request.UserId,
            IncludeProperties: "Category");
        List<string> categories = prefernces.Select(x => x.Category.CategoryId).ToList();
        return null;
    }
}
