using Demo03.Notifications.Worker.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Demo03.Notifications.Worker.Services
{
    public sealed class EmailSender
    {
        private readonly SmtpOptions _opt;

        public EmailSender(IOptions<SmtpOptions> opt)
            => _opt = opt.Value;

        public async Task SendAsync(string subject, string body, CancellationToken ct)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_opt.From));
            message.To.Add(MailboxAddress.Parse(_opt.To));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_opt.Host, _opt.Port, useSsl: false, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }
    }
}
