using DevTalk.Application.Services;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DevTalk.Application.ApplicationUser.Commands.GoogleSingInCallback;

public class GoogleSingInCallbackCommandHandler(SignInManager<User> signInManager,
    UserManager<User> userManager,IAuthService authService) : IRequestHandler<GoogleSingInCallbackCommand,
    AuthResponse>
{
    public async Task<AuthResponse> Handle(GoogleSingInCallbackCommand request, CancellationToken cancellationToken)
    {
        if (request.RemoteError != null)
        {
            throw new CustomeException($"Error from external provider: {request.RemoteError}");
        }

        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            throw new CustomeException("Error loading external login information.");
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var name = info.Principal.FindFirstValue(ClaimTypes.Name);
        var picture = info.Principal.FindFirstValue("urn:google:picture");

        if (email is null || name is null)
            throw new CustomeException("Error loading external login information.");

        var user = await userManager.FindByEmailAsync(email);


        if(user is null)
        {
            var username = email.Split('@')[0];
            user = new User
            {
                Email = email,
                FirstName = name,
                UserName = username,
                AvatarUrl = picture,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                throw new CustomeException("Could not create local user.");
            }
            await userManager.AddToRoleAsync(user, UserRoles.User);
            var loginResult = await userManager.AddLoginAsync(user, info);
            if (!loginResult.Succeeded)
            {
                throw new CustomeException("Could not add external login info to user.");
            }
        }
        else
        {
            bool updated = false;
            if (user.FirstName != name)
            {
                user.FirstName = name;
                updated = true;
            }
            if (user.AvatarUrl != picture)
            {
                user.AvatarUrl = picture;
                updated = true;
            }

            if (updated)
            {
                await userManager.UpdateAsync(user);
            }
        }

        var roles = await userManager.GetRolesAsync(user);
        var authResponse = await authService.GetJwtToken(user, roles.ToList());
        return authResponse;
    }
}
