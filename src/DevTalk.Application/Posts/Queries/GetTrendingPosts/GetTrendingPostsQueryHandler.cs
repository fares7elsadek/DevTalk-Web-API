using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetTrendingPosts;

public class GetTrendingPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetTrendingPostsQuery, IEnumerable<PostDto>>
{
    public async Task<IEnumerable<PostDto>> Handle(GetTrendingPostsQuery request, CancellationToken cancellationToken)
    {
        int page = request.page;
        int size = request.size;

        if (page < 1) page = 1;

        
        var popularNow = await unitOfWork.Post.GetAllWithConditionAsync(
            post => post.PostedAt >= DateTime.UtcNow.AddHours(-24),
            IncludeProperties: "Comments,Votes"
        );

        int total = popularNow.Count();
        if (total == 0) return Enumerable.Empty<PostDto>(); 
        if (size > total) size = total;

        int totalPages = (int)Math.Ceiling((decimal)total / size);
        if (page > totalPages) page = totalPages;

        
        var result = popularNow.Select(post => new
        {
            Post = post,
            PopularityScore =
                (post.Votes.Count(v => v.VoteType == VoteType.UpVote) * 2) +
                (post.Comments.Count) -
                (post.Votes.Count(v => v.VoteType == VoteType.DownVote) * 1.5)
        })
        .OrderByDescending(p => p.PopularityScore)
        .Skip((page - 1) * size)
        .Take(size)
        .Select(p => p.Post) 
        .ToList();

        
        var postsDto = mapper.Map<IEnumerable<PostDto>>(result);
        return postsDto;
    }
}

