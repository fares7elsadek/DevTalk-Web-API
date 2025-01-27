using DevTalk.Domain.Entites;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
using System.Text;

namespace DevTalk.Application.ApplicationUser.Commands.ResetPassword;

public class ResetPasswordCommandHandler(UserManager<User> userManager) : IRequestHandler<ResetPasswordCommand, string>
{
    public async Task<string> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId!);
        if (user == null)
            return "Invalid password reset attempt.";


        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token!));

        var result = await userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            Log.Error($"Failed to reset password for user {user.Email}: {errors}");
            return $"Password reset failed: {errors}";
        }

        user.ForgetPasswordResetLinkRequestedAt = null;
        await userManager.UpdateAsync(user);

        Log.Information($"Password successfully reset for user {user.Email}.");
        return "Your password has been successfully reset.";
    }
}
