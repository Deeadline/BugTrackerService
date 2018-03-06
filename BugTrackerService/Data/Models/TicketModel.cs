using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Data.Models
{
    public class TicketModel
    {
        [Display(Name = "Number")]
        [Key]
        public int TicketID { get; set; }
        [Display(Name = "Created by")]
        public int UserID { get; set; }
        [Display(Name = "Assigned")]
        [DisplayFormat(NullDisplayText = "No one is assigned to this ticket")]
        public int? EmployeeID { get; set; }
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
        public EmployeeModel Employee { get; set; }
        [Display(Name = "Comments")]
        [DisplayFormat(NullDisplayText = "No comments for this Ticket")]
        public ICollection<CommentModel> Comments { get; set; }
    }
}
