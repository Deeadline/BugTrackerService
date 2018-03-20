using System.Threading.Tasks;

namespace BugTrackerService.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
