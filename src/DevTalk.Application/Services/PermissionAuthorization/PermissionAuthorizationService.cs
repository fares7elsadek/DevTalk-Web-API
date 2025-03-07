using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Helpers;
using DevTalk.Domain.Repositories;
using System.Security;

namespace DevTalk.Application.Services.PermissionAuthorization;

public class PermissionAuthorizationService(IUserContext context) 
    : IPermissionAuthorizationService
{
    public bool Authorize(string userId, Permissions permission, object resource)
    {
        var currentUser = context.GetCurrentUser();
        bool isAdmin = currentUser.IsInRole(UserRoles.Admin);

        // Post
        if (resource is Post post && (permission == Permissions.DeletePost ||
            permission == Permissions.UpdatePost))
        {
            return isAdmin || post.UserId == userId;
        }

        // Comment
        if (resource is Comment comment && (permission == Permissions.DeleteComment||
            permission == Permissions.UpdateComment))
        {
            return isAdmin || comment.UserId == userId;
        }

        // delete bookmark
        if (resource is Bookmarks bookmark && permission == Permissions.DeleteBookmark)
        {
            return isAdmin || bookmark.UserId == userId;
        }

        // delete preference
        if (resource is Preference preference && permission == Permissions.DeletePreference)
        {
            return isAdmin || preference.UserId == userId;
        }

        return false;
    }
}
