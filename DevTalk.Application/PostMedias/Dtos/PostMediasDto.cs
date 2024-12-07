using DevTalk.Domain.Constants;

namespace DevTalk.Application.PostMedias.Dtos;

public class PostMediasDto
{
    public string PostMediaId { get; set; } = default!;
    public PostMediaTypes Type { get; set; }
    public string MediaPath { get; set; } = default!;
    public string PostId { get; set; } = default!;
}
