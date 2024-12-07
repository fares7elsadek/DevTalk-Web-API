using DevTalk.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DevTalk.Application.Services;

public class FileService(IWebHostEnvironment environment) : IFileService
{
    public void DeleteFile(string FileName)
    {
        if (string.IsNullOrEmpty(FileName))
        {
            throw new ArgumentNullException(nameof(FileName));
        }
        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, $"Uploads", FileName);

        if (!File.Exists(path))
        {
            throw new CustomeException($"Invalid file path");
        }
        File.Delete(path);
    }

    public async Task<string> SaveFileAsync(IFormFile file, string[] allowedExtensions)
    {
        if(file == null) throw new ArgumentNullException(nameof(file));

        var ContentPath = environment.ContentRootPath;
        var path = Path.Combine(ContentPath, "Uploads");
        
        if(!Directory.Exists(path)) Directory.CreateDirectory(path);
        var ext = Path.GetExtension(file.FileName);
        if (!allowedExtensions.Contains(ext))
        {
            throw new CustomeException($"Only {string.Join(",", allowedExtensions)} are allowed.");
        }

        var fileName = $"{Guid.NewGuid().ToString()}{ext}";
        var fileNameWithPath = Path.Combine(path, fileName);
        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await file.CopyToAsync(stream);
        return fileName;
    }
}
