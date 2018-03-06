using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Data.Models
{
    public class CommentModel
    {
        [Key]
        [Display(Name = "Number")]
        public int CommentID { get; set; }
        public int TicketID { get; set; }
        [Required]
        public string Description { get; set; }

        public TicketModel Ticket { get; set; }
    }
}
