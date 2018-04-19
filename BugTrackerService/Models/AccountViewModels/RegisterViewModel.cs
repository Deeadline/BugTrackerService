using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [RegularExpression(@"(^[\S]*(?=.*[a-z])(?=.*[A-Z])(?=.*[\d])(?=.*[\W_])[\S]{8,15}$)")]
        [Required]
        [StringLength(100)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        [StringLength(20)]
        public string WorkerCardNumber { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
