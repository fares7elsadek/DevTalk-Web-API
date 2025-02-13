namespace DevTalk.Application.Posts.Dtos;

public class GetUserPostsDto
{
    public GetUserPostsDto()
    {
        Posts = new HashSet<PostDto>();
    }
    public string Id_cursor { get; set; } = default!;
    public string time_cursor { get; set; } = default!;
    public double score_cursor { get; set; } = default!;
    public IEnumerable<PostDto> Posts { get; set; }
}
