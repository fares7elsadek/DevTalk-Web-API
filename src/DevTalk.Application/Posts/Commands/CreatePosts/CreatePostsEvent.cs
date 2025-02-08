using MediatR;
using Microsoft.AspNetCore.Http;

namespace DevTalk.Application.Posts.Commands.CreatePosts;

public class CreatePostsEvent:INotification
{
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public List<IFormFile>? Files { get; set; }
}
