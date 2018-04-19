using System.Threading.Tasks;

namespace BugTrackerService.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: {link}");
        }

        public static Task SendEmailUpdateAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Your ticket has been updated",
                $"Your ticket has been updated, to see the changes click this link: {link}");
        }
    }
}
