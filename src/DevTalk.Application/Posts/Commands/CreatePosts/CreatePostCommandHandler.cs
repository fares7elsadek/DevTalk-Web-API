using AutoMapper;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Application.Services;
using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Posts.Commands.CreatePosts;

public class CreatePostCommandHandler(IMapper mapper,
    IUnitOfWork unitOfWork,IFileService fileService,
    IUserContext userContext) : IRequestHandler<CreatePostCommand, PostDto>
{
    public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        if (user == null) throw new CustomeException("The user is not authroized");
        var post = new Post
        {
            Title = request.Title,
            Body = request.Body,
            UserId = user.userId
        };

        if (request.Files == null || request.Files.Count == 0)
        {
            await unitOfWork.Post.AddAsync(post);
            await unitOfWork.SaveAsync();
            var postDto = mapper.Map<PostDto>(post);
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
            return postDto;
        }
    }
}
