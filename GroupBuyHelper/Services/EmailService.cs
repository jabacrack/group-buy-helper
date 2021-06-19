using System.Threading.Tasks;
using GroupBuyHelper.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GroupBuyHelper.Services
{
    public class EmailService : IEmailSender
    {
        private readonly EmailSenderOptions options;

        public EmailService(IOptions<EmailSenderOptions> options)
        {
            this.options = options.Value;
        }

        public EmailSenderOptions Options { get; } //set only via Secret Manager

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            var fromAddress = new MailboxAddress(name: options.Sender, address: options.EmailAddress);
            emailMessage.From.Add(fromAddress);

            var toAddress = new MailboxAddress(name: "", address: email);
            emailMessage.To.Add(toAddress);

            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            bodyBuilder.TextBody = message;

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(options.Host, options.Port, options.UseSSL);
            await client.AuthenticateAsync(options.Login,options.Password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}