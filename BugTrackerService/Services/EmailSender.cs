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
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var from = new MailAddress("mikolaj.szymanski.lodz@gmail.com");
            var password = "Zaq1Xsw2CDA1";
            var to = new MailAddress(email);
            SmtpClient smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, password)
            };
            using (var messages = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message
            })
            smtp.Send(messages);
            smtp.PickupDirectoryLocation = @"E:\Licencjat\Emails";
            return Task.CompletedTask;
        }
    }
}
