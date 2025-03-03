﻿using AutoMapper;
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

namespace DevTalk.Application.Posts.Commands.CreatePosts;

public class CreatePostCommandHandler(IMapper mapper,
    IUnitOfWork unitOfWork,IFileService fileService,
    IUserContext userContext,IPublisher publisher,IPublishEndpoint publishEndpoint) : IRequestHandler<CreatePostCommand, PostDto>
{
    public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        if (user == null) throw new CustomeException("The user is not authroized");

        Post post;
        if(request.Categories is not null)
        {
            var categories = await unitOfWork.Category.GetAllWithConditionAsync(x => request.Categories.Contains(x.CategoryId)
            ,IncludeProperties: "Preferences");
            if (request.Categories.Count != categories.ToList().Count)
                throw new CustomeException("One or more selected categories are invalid.");
            post = new Post
            {
                Title = request.Title,
                Body = request.Body,
                UserId = user.userId,
                Categories = categories.ToList(),
            };
            List<string> SubscribedUsers = new List<string>();
            foreach(var category in categories)
            {
                foreach(var prefernces in category.Preferences)
                {
                    SubscribedUsers.Add(prefernces.UserId);
                }
            }

            await publishEndpoint.Publish(new NotificationMessage
            {
                UserIds = SubscribedUsers,
                Content = $"New post published: {request.Title}",
                Timestamp = DateTime.UtcNow
            });

        }
        else
        {
            post = new Post
            {
                Title = request.Title,
                Body = request.Body,
                UserId = user.userId,
            };
        }
        
        if (request.Files == null || request.Files.Count == 0)
        {
            await unitOfWork.Post.AddAsync(post);
            await unitOfWork.SaveAsync();
            var postDto = mapper.Map<PostDto>(post);
            await publisher.Publish(new CreatePostsEvent
            {
                Title= request.Title,
                Body= request.Body,
            });
            return postDto;
        }
        else
        {
            if(request.Files.Count > 5)
                throw new CustomeException("The maximum number of photos is 5");
            var maxFileSize = 5 * 1024 * 1024;
            foreach(var file in request.Files)
            {
                if(file.Length > maxFileSize)
                {
                    throw new CustomeException("The maximum file size is 5 MB");
                }
            }
            HashSet<PostMedia> postMedias = new HashSet<PostMedia>();
            foreach(var file in request.Files)
            {
                if(file.Length > 0)
                {
                    PostMedia newPostMedia = new();
                    var ext = Path.GetExtension(file.FileName);
                    if (ext == ".mp4") newPostMedia.Type = PostMediaTypes.Video;
                    else newPostMedia.Type = PostMediaTypes.Image;
                    string[] allowedImageExtensions = new string[]
                    {
                        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" 
                    };
                    var mediaPath = await fileService.SaveFileAsync(file, allowedImageExtensions);
                    if (string.IsNullOrEmpty(mediaPath))
                        throw new CustomeException("somthing wrong has happend");
                    newPostMedia.MediaPath = mediaPath;
                    postMedias.Add(newPostMedia);
                }
            }
            post.PostMedias= postMedias;
            await unitOfWork.Post.AddAsync(post);
            await unitOfWork.SaveAsync();
            var postDto = mapper.Map<PostDto>(post);
            await publisher.Publish(new CreatePostsEvent
            {
                Title = request.Title,
                Body = request.Body,
                Files = request.Files,
            });
            return postDto;
        }
    }
}
