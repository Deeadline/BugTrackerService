using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
