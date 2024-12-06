using System.Net;

namespace DevTalk.Domain.Helpers;

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public bool IsSuccess { get; set; } = true;

    public List<string> Errors { get; set; } = new List<string>();

    public object Result { get; set; }
}
