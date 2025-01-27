using DevTalk.Application.Posts.Dtos;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetAllPosts;

public class GetAllPostsQuery : IRequest<IEnumerable<PostDto>>
{
}
