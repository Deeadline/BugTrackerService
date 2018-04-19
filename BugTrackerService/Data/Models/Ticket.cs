using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Data.Models
{
    public class Ticket
    {
        [Key]
        [Display(Name = "ID")]
        public int TicketId { get; set; }
        [Display(Name = "Created by")]
        public string OwnerId { get; set; }
        public User Owner { get; set; }
        [Display(Name = "Assigned to")]
        [DisplayFormat(NullDisplayText = "No one is assigned to this ticket")]
        public string EmployeeId { get; set; }
        public User Employee { get; set; }
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Display(Name = "Priority")]
        public int PriorityId { get; set; }
        public Priority Priority { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 6)]
        public string Title { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 6)]

        public string Description { get; set; }

        [Display(Name = "Create Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:f}")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Update Date")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:f}")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }

        [Display(Name = "Comments")]
        [DisplayFormat(NullDisplayText = "No comments for this Ticket")]
        public List<Comment> Comments { get; set; }

        public bool Assigned { get; set; }
        public List<FileDetail> FileDetails { get; set; }
    }
}
