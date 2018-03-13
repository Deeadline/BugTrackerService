using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        public string WorkerCardNumber { get; set; }
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
        [DisplayFormat(NullDisplayText = "No tickets")]
        [Display(Name ="Tickets")]
        public ICollection<Ticket> Tickets { get; set; }

    }
}
