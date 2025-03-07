using Microsoft.AspNetCore.Http;

namespace DevTalk.Application.Services;

public interface IFileService
{
    Task<(string,string)> SaveFileAsync(IFormFile file, string[] allowedExtensions);
    Task DeleteFile(string FileName);
}
