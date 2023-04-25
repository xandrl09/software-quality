using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Stocks.Services.Models.Configuration;

namespace Stocks.Services.Export
{
    public class EmailExportService: IExportService
    {
        private readonly Settings _settings;

        public EmailExportService(Settings settings)
        {
            _settings = settings;
        }

        private MimeMessage CreateBaseMailMessage()
        {
            var mailMessage = new MimeMessage();
            
            mailMessage.From.Add(new MailboxAddress("Stocks",_settings.Email.Sender));

            foreach (var recepient in _settings.Email.Recepients)
            {
                mailMessage.To.Add(new MailboxAddress(recepient, recepient));
            }

            mailMessage.Subject = string.Format(_settings.Email.SubjectTemplate, DateTime.Today.ToShortDateString());

            return mailMessage;
        }

        private MimeEntity CreateBody(string content)
        {
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = content
            };
            return bodyBuilder.ToMessageBody();
        }

        public async Task Export(string content)
        {
            var message = CreateBaseMailMessage();

            message.Body = CreateBody(content);

            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.Smtp.Host, _settings.Smtp.Port, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_settings.Smtp.Username, _settings.Smtp.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return;
        }
    }
}