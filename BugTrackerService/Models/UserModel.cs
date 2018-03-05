using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Models
{
    public class UserModel
    {
        [Key]
        [Display(Name = "Number")]
        public int UserID { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string EMail { get; set; }
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
        [NotMapped]
        [DisplayFormat(NullDisplayText = "No tickets")]
        public List<TicketModel> Tickets { get; set; }

    }
}
