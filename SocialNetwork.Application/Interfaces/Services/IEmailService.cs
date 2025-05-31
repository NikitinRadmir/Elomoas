using MailKit.Net.Smtp;
using MimeKit;

namespace Elomoas.Application.Interfaces.Services;

public interface IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string message);
    public Task SendMessage(MimeMessage message);
}
