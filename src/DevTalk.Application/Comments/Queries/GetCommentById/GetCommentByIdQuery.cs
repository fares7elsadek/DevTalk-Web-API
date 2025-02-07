using DevTalk.Application.Caching;
using DevTalk.Application.Comments.Dtos;
using MediatR;

namespace DevTalk.Application.Comments.Queries.GetCommentById;

public class GetCommentByIdQuery(string commentId,string postId):ICachableRequest<CommentDto>
{
    public string CommentId { get; set; } = commentId;
    public string PostId { get; set; } = postId;
    public string Key => $"post:{PostId}:comment:{CommentId}";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
