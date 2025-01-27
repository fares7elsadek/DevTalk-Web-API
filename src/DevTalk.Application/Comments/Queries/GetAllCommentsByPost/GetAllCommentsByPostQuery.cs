using DevTalk.Application.Comments.Dtos;
using MediatR;

namespace DevTalk.Application.Comments.Queries.GetAllCommentsByPost;

public class GetAllCommentsByPostQuery(string postId):IRequest<IEnumerable<CommentDto>>
{
    public string PostId { get; set; } = postId;
}
