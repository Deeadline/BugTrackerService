using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Models
{
    public class EmployeeModel
    {
        [Key]
        [Display(Name = "Number")]
        public int EmployeeModelID { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public String FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public String LastName { get; set; }
        [Required]
        [EmailAddress]
        public String EMail { get; set; }
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String Password { get; set; }
        [Required]
        [StringLength(100)]
        public String CompanyName { get; set; }
        [Required]
        [Phone]
        public String PhoneNumber { get; set; }

        public String FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
        [NotMapped]
        public int? TicketID { get; set; }
        [NotMapped]
        public ICollection<TicketModel> Tickets { get; set; }
    }
}
