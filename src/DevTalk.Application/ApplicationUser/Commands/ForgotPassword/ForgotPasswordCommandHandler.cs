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

namespace DevTalk.Application.ApplicationUser.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(UserManager<User> userManager,
    IEmailSender<User> emailSender,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<ForgotPasswordCommand, string>
{
    public async Task<string> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return "If the email exists, you will receive a reset link shortly.";

        if (!await userManager.IsEmailConfirmedAsync(user))
            return "Your email is not confirmed.";


        if (user.ForgetPasswordResetLinkRequestedAt != null &&
            (DateTime.UtcNow - user.ForgetPasswordResetLinkRequestedAt.Value).TotalMinutes < 15)
        {
            return "You have already requested a password reset recently. Please check your email.";
        }


        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        if (httpContextAccessor.HttpContext == null)
        {
            Log.Error($"Failed to send password reset link email for user {user.Email} becasue of httpcontext is null");
            throw new CustomeException($"Failed to send password reset link");
        }
        var routeValues = new RouteValueDictionary()
        {
            ["UserId"] = user.Id,
            ["Token"] = encodedCode
        };

        var confirmEmailUrl = $"https://dev-talk-phi.vercel.app/reset-password?userId={user.Id}&token={encodedCode}";


        user.ForgetPasswordResetLinkRequestedAt = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        try
        {
            await emailSender.SendPasswordResetLinkAsync(user, request.Email, confirmEmailUrl);
            Log.Information($"Password reset email sent to {user.Email}.");
            return "If the email exists, you will receive a reset link shortly.";
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to send password reset code email for user {user.Email}: {ex}");
            throw new CustomeException($"Failed to send password reset code email: {ex.Message}");
        }
    }
}
