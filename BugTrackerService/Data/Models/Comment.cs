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
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:f}")]
        public DateTime SendTime { get; set; }

        [Display(Name = "Author")]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
