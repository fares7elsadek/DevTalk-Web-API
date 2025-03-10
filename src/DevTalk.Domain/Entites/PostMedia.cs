﻿using DevTalk.Domain.Constants;

namespace DevTalk.Domain.Entites;

public class PostMedia
{
    public string PostMediaId { get; set; } = default!;
    public PostMediaTypes Type { get; set;}
    public string MediaUrl { get; set; } = default!;
    public string MediaFileName { get; set; } = default!;
    public string PostId { get; set; } = default!;
    public Post Post { get; set; } = default!;
}
