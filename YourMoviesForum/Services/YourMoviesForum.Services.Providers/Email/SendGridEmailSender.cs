
namespace YourMoviesForum.Services.Providers.Email
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    using static YourMoviesForum.Common.GlobalConstants;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public SendGridEmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
             =>this.Execute(this.configuration.GetSection("SendGrid:ApiKey").Value, email, subject, htmlMessage);
     

        private Task Execute(string apiKey, string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(apiKey);
            var message = new SendGridMessage()
            {
                From = new EmailAddress(SystemEmail, SystemName),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage,
            };

            message.AddTo(new EmailAddress(email));
            message.SetClickTracking(false, false);
            return client.SendEmailAsync(message);
        }
    }
}
