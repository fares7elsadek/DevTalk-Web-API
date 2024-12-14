using DevTalk.Application.Posts.Dtos;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetTrendingPosts;

public class GetTrendingPostsQuery(int page,int size):IRequest<IEnumerable<PostDto>>
{
    public int page { get; set; } = page;
    public int size { get; set; } = size;
}
