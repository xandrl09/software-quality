using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Stocks.Services.Models.Configuration;

namespace Stocks.Services.Export
{
    /// <summary>
    /// Class <c>EmailExportService</c> defines the service that exports content via email.
    /// </summary>
    public class EmailExportService : IExportService
    {
        private readonly Settings _settings;

        /// <summary>
        /// Creates a new instance of <see cref="EmailExportService"/>.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public EmailExportService(Settings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Creates the mail message with the base configuration from the settings.
        /// </summary>
        /// <returns><c>MimeMessage</c> object.</returns>
        private MimeMessage CreateBaseMailMessage()
        {
            var mailMessage = new MimeMessage();

            mailMessage.From.Add(new MailboxAddress("Stocks", _settings.Email.Sender));

            foreach (var recipient in _settings.Email.Recepients)
            {
                mailMessage.To.Add(new MailboxAddress(recipient, recipient));
            }

            mailMessage.Subject = string.Format(_settings.Email.SubjectTemplate, DateTime.Today.ToShortDateString());

            return mailMessage;
        }

        /// <summary>
        /// Creates the body of the email.
        /// </summary>
        /// <param name="content">The content of the email.</param>
        /// <returns><c>MimeEntity</c> object.</returns>
        private MimeEntity CreateBody(string content)
        {
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = content
            };
            return bodyBuilder.ToMessageBody();
        }

        /// <summary>
        /// Sends the content via email asynchronously.
        /// </summary>
        /// <param name="content">The content to be sent.</param>
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