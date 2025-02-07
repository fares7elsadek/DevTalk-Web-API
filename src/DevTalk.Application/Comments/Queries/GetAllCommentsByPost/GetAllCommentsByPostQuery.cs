using DevTalk.Application.Caching;
using DevTalk.Application.Comments.Dtos;
using MediatR;

namespace DevTalk.Application.Comments.Queries.GetAllCommentsByPost;

public class GetAllCommentsByPostQuery(string postId):ICachableRequest<IEnumerable<CommentDto>>
{
    public string PostId { get; set; } = postId;
    public string Key => $"post:{PostId}:comment:all";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
