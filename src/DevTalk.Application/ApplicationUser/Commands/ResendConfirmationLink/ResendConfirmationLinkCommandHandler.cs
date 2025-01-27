using DevTalk.Application.Services.EmailService;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
using System.Text;

namespace DevTalk.Application.ApplicationUser.Commands.ResendConfirmationLink;

public class ResendConfirmationLinkCommandHandler(UserManager<User> userManager,
    LinkGenerator linkGenerator,IEmailSender<User> emailSender,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<ResendConfirmationLinkCommand, string>
{
    public async Task<string> Handle(ResendConfirmationLinkCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new CustomeException("Email is not registered");
        if(await userManager.IsEmailConfirmedAsync(user))
            throw new CustomeException("Email is already confirmed");

        if(httpContextAccessor.HttpContext == null)
            throw new CustomeException("Something wrong has happened");

        try
        {
            await SendConfirmationEmailAsync(user, httpContextAccessor.HttpContext);
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to resend confirmation email for user {user.Email}: {ex.Message}");
            throw new CustomeException($"Failed to resend confirmation email for user {user.Email}: {ex.Message}");
        }
        return "Email sent successfully";
    }
    private async Task SendConfirmationEmailAsync(User user, HttpContext httpContext)
    {
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        user.LastEmailConfirmationToken = code;
        await userManager.UpdateAsync(user);
        var userId = await userManager.GetUserIdAsync(user);
        var routeValues = new RouteValueDictionary()
        {
            ["userId"] = userId,
            ["code"] = code
        };
        var confirmEmailEndpointName = "ConfirmEmail";
        var confirmEmailUrl = linkGenerator.GetUriByName(httpContext, confirmEmailEndpointName, routeValues)
            ?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");

        if (user.Email == null)
            throw new CustomeException("User email is not defined");

        await emailSender.SendConfirmationLinkAsync(user, user.Email, confirmEmailUrl);
    }
}
