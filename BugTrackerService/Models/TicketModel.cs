using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Models
{
    public class TicketModel
    {
        [Display(Name = "Number")]
        [Key]
        public int TicketID { get; set; }
        public int UserID { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 6)]
        public string Title { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 6)]
        public string Description { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        [Display(Name = "Create Date")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Update Date")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }
        public UserModel User { get; set; }
        [DisplayFormat(NullDisplayText = "No one is assigned to this ticket")]
        public List<EmployeeModel> Employees { get; set; }
    }
}
