using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
