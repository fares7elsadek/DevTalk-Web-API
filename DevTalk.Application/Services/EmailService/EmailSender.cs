using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using DevTalk.Domain.Helpers;
using Serilog;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Entites;
using Microsoft.AspNetCore.Identity;

namespace DevTalk.Application.Services.EmailService;

public class EmailSender(IOptions<EmailOptions> options) : IEmailSender<User>
{
    private readonly string _smtpEmail = options.Value.smptemail;
    private readonly string _smtpPassword = options.Value.smptpassword;
    private readonly string _smtpHost = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly bool _enableSsl = true;
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(_smtpEmail),
            Subject = "Confirm your email",
            Body = EmailTemplate.ConfirmEmail(confirmationLink),
            IsBodyHtml = true
        };

        message.To.Add(email);

        using var smtpClient = new SmtpClient(_smtpHost, _smtpPort)
        {
            Credentials = new NetworkCredential(_smtpEmail, _smtpPassword),
            EnableSsl = _enableSsl,
        };

        try
        {
            await smtpClient.SendMailAsync(message);
        }
        catch (SmtpException smtpEx)
        {
            Log.Fatal($"SMTP Error: {smtpEx.Message}");
            throw new CustomeException($"SMTP Error: {smtpEx.Message}");
        }
        catch (Exception ex)
        {
            Log.Fatal($"SMTP Error: {ex.Message}");
            throw new CustomeException($"SMTP Error: {ex.Message}");
        }
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }
}
