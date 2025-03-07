using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Attributes;
using DevTalk.Application.Bookmark.commands.DeleteBookmark;
using DevTalk.Application.Comments.Commands;
using DevTalk.Application.Posts.Commands;
using DevTalk.Application.Preferences.Commands.DeletePreference;
using DevTalk.Application.Services.PermissionAuthorization;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using System.Reflection;

namespace DevTalk.Application.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(IUserContext context,
    IUnitOfWork unitOfWork,IPermissionAuthorizationService service)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authAttribute = typeof(TRequest).GetCustomAttribute<HasPermissionAttribute>();
        if (authAttribute != null)
        {
            var permission = authAttribute.Permission;
            string userId = context.GetCurrentUser().userId;
            object resource = await LoadResourceForRequestAsync(request,userId);
            if (!service.Authorize(userId, permission, resource))
                throw new CustomeException("User not authorized");
        }
        return await next();
    }

    private async Task<object> LoadResourceForRequestAsync(TRequest request,string userId)
    {
        //Post
        if(request is IPostCommand<string> postResource)
        {
            var post = await unitOfWork.Post.GetOrDefalutAsync(x => x.PostId == postResource.ResourceId);
            if (post is null)
                throw new NotFoundException(nameof(post), postResource.ResourceId);
            return post;
        }

        //Comment
        if (request is ICommentCommand<string> commentResource)
        {
            var comment = await unitOfWork.Comment.GetOrDefalutAsync(x => x.CommentId == commentResource.ResourceId);
            if (comment is null)
                throw new NotFoundException(nameof(comment), commentResource.ResourceId);
            return comment;
        }

        //Bookmark
        if(request is DeleteBookmarkCommand deleteBookmark)
        {
            var bookmark = await unitOfWork.Bookmark.GetOrDefalutAsync(x => (x.PostId == deleteBookmark.PostId) && (x.UserId == userId) );
            if (bookmark is null)
                throw new NotFoundException(nameof(bookmark), deleteBookmark.PostId);
            return bookmark;
        }


        //prefernce
        if (request is DeletePreferenceCommand deletePrefernce)
        {
            var prefernce = await unitOfWork.Preference.GetOrDefalutAsync(x => (x.CategoryId == deletePrefernce.CategoryId) &&(x.UserId == userId));
            if (prefernce is null)
                throw new NotFoundException(nameof(prefernce), deletePrefernce.CategoryId);
            return prefernce;
        }


        throw new CustomeException("User not authorized");

    }
}
