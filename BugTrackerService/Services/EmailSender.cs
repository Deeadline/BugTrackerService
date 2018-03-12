using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BugTrackerService.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public EmailSettings _emailSettings { get; }
        public Task SendEmailAsync(string email, string title, string message)
        {
            SmtpClient client = new SmtpClient();
            client.Port = _emailSettings.PrimaryPort;
            client.Host = _emailSettings.PrimaryDomain;


            client.EnableSsl = true;
            //client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
            MailMessage mail2 = new MailMessage();

            mail2.From = new MailAddress(_emailSettings.UsernameEmail, "Bug Tracker Service");
            mail2.To.Add(new MailAddress(email));
            return Task.CompletedTask;
        }
    }
}
