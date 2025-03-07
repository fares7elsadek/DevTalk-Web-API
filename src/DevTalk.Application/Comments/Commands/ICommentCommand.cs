namespace DevTalk.Application.Comments.Commands;

public interface ICommentCommand<TResourceId>
{
    TResourceId ResourceId { get; }
}
