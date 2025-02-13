using DevTalk.Application.Caching;
using DevTalk.Application.Comments.Dtos;
using MediatR;

namespace DevTalk.Application.Comments.Queries.GetAllCommentsByPost;

public class GetAllCommentsByPostQuery(string postId,int page,int size):ICachableRequest<IEnumerable<CommentDto>>
{
    public int Page { get; set; } = page;
    public int Size { get; set; } = size;
    public string PostId { get; set; } = postId;
    public string Key => $"post:{PostId}:comment:all";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
