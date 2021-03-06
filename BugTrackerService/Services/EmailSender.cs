﻿using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BugTrackerService.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var from = new MailAddress("");
            var password = "";
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
            smtp.PickupDirectoryLocation = @"\Emails";
            return Task.CompletedTask;
        }
    }
}
