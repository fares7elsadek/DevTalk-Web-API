using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Application.Services;
using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using MassTransit;
using DevTalk.Application.Notification.MessageQueue;
using Microsoft.AspNetCore.Http;

namespace DevTalk.Application.Posts.Commands.CreatePosts;

public class CreatePostCommandHandler(IMapper mapper,
    IUnitOfWork unitOfWork, IFileService fileService,
    IUserContext userContext, IPublisher publisher, IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreatePostCommand, PostDto>
{
    public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        if (user == null)
            throw new CustomeException("The user is not authorized");

        var post = new Post
        {
            Title = request.Title,
            Body = request.Body,
            UserId = user.userId
        };

        if (request.Categories?.Count > 0)
        {
            var categories = await unitOfWork.Category
                .GetAllWithConditionAsync(x => request.Categories.Contains(x.CategoryId),
                IncludeProperties: "Preferences");

            if (request.Categories.Count != categories.Count())
                throw new CustomeException("One or more selected categories are invalid.");

            post.Categories = categories.ToList();

            var subscribedUsers = categories
                .SelectMany(category => category.Preferences.Select(p => p.UserId))
                .ToHashSet(); 

            if (subscribedUsers.Count > 0)
            {
                await publishEndpoint.Publish(new NotificationMessage
                {
                    UserIds = subscribedUsers.ToList(),
                    Content = $"New post published: {request.Title}",
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        
        if (request.Files?.Count > 0)
        {
            if (request.Files.Count > 5)
                throw new CustomeException("The maximum number of photos is 5");

            post.PostMedias = await ProcessFilesAsync(request.Files);
        }

        await unitOfWork.Post.AddAsync(post);
        await unitOfWork.SaveAsync(); 

        var postDto = mapper.Map<PostDto>(post);
        await publisher.Publish(new CreatePostsEvent
        {
            Title = request.Title,
            Body = request.Body,
            Files = request.Files
        });

        return postDto;
    }

    private async Task<HashSet<PostMedia>> ProcessFilesAsync(List<IFormFile> files)
    {
        var maxFileSize = 5 * 1024 * 1024;
        var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        var postMedias = new HashSet<PostMedia>();

        foreach (var file in files)
        {
            if (file.Length > maxFileSize)
                throw new CustomeException("The maximum file size is 5 MB");

            if (file.Length > 0)
            {
                var ext = Path.GetExtension(file.FileName);
                var postMedia = new PostMedia
                {
                    Type = ext == ".mp4" ? PostMediaTypes.Video : PostMediaTypes.Image
                };

                var (url, filename) = await fileService.SaveFileAsync(file, allowedImageExtensions);
                if (string.IsNullOrEmpty(url))
                    throw new CustomeException("Something went wrong while uploading the file");

                postMedia.MediaUrl = url;
                postMedia.MediaFileName = filename;
                postMedias.Add(postMedia);
            }
        }
        return postMedias;
    }
}
