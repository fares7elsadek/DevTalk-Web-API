using DevTalk.Application.Posts.Dtos;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetPostById;

public class GetPostByIdQuery(string Id):IRequest<PostDto>
{
    public string PostId { get; set; } = Id;
}
