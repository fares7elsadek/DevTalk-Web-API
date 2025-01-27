using DevTalk.Application.Comments.Dtos;
using MediatR;

namespace DevTalk.Application.Comments.Queries.GetCommentById;

public class GetCommentByIdQuery(string commentId,string postId):IRequest<CommentDto>
{
    public string CommentId { get; set; } = commentId;
    public string PostId { get; set; } = postId;
}
