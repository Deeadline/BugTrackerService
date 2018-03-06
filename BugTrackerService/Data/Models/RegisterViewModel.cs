using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Data.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30)]
        public String Name { get; set; }
        [Required]
        [StringLength(50)]
        public String Surname { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public String Email { get; set; }
        [RegularExpression(@"(^[\S]*(?=.*[a-z])(?=.*[A-Z])(?=.*[\d])(?=.*[\W_])[\S]{8,15}$)")]
        [Required]
        [StringLength(100)]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public String ConfirmPassword { get; set; }
        [Required]
        [StringLength(100)]
        public String CompanyName { get; set; }
        [Required]
        [Phone]
        public String PhoneNumber { get; set; }
    }
}
