namespace DevTalk.Application.Posts.Dtos;

public class GetAllPostsDto
{
    public GetAllPostsDto()
    {
        Posts = new HashSet<PostDto>();
    }
    public string Id_cursor { get; set; } = default!;
    public IEnumerable<PostDto> Posts { get; set; } 
}
