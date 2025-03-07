namespace DevTalk.Application.Posts.Commands;

public interface IPostCommand<TResourceId>
{
    TResourceId ResourceId { get; }
}
