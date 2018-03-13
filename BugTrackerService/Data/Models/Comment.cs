using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Data.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public int TicketID { get; set; }
        [Required]
        public string Content { get; set; }

        public string SendTime { get; set; }
        [Display(Name = "Author")]
        public int UserId { get; set; }

        public User User { get; set; }

        public Ticket Ticket { get; set; }
    }
}
