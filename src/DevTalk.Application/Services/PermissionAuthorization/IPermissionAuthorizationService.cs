using DevTalk.Domain.Constants;
using DevTalk.Domain.Helpers;

namespace DevTalk.Application.Services.PermissionAuthorization;

public interface IPermissionAuthorizationService
{
    bool Authorize(string userId ,Permissions permission, object resource);
}
