using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Data.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public int TicketID { get; set; }
        [Required]
        public string Content { get; set; }

        public DateTime SendTime { get; set; }
        [Display(Name = "Author")]
        public string UserId { get; set; }

        public User User { get; set; }

        public Ticket Ticket { get; set; }
    }
}
