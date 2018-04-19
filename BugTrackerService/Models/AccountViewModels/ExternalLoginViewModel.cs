using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
