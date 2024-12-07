using DevTalk.Application.Posts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace DevTalk.Application.Posts.Commands.CreatePosts;

public class CreatePostCommand:IRequest<PostDto>
{
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public List<IFormFile>? Files { get; set; } 
}
