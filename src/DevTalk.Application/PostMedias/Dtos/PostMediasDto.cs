using DevTalk.Domain.Constants;

namespace DevTalk.Application.PostMedias.Dtos;

public class PostMediasDto
{
    public string PostMediaId { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string MediaUrl { get; set; } = default!;
}
