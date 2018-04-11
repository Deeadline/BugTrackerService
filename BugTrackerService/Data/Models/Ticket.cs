using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Data.Models
{
    public class Ticket
    {
        private DateTime _createDate;
        private DateTime _updateDate;
        private Priority _priority;
        private int _id;

        [Key]
        [Display(Name = "ID")]
        public int TicketId { get => _id; set => _id = value; }
        [Display(Name = "Created by")]
        public string OwnerId { get; set; }
        public User Owner { get; set; }
        [Display(Name = "Assigned to")]
        [DisplayFormat(NullDisplayText = "No one is assigned to this ticket")]
        public string EmployeeId { get; set; }
        public User Employee { get; set; }
        [Display(Name = "Product")]
        public int? ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 6)]
        public string Title { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 6)]

        public string Description { get; set; }

        public Status Status { get; set; }

        public Priority Priority { get => _priority; set => _priority = value; }

        [Display(Name = "Create Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:f}")]
        public DateTime CreateDate { get => _createDate; set => _createDate = DateTime.Now; }

        [Display(Name = "Update Date")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:f}")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get => _updateDate; set => _updateDate = DateTime.Now; }

        [Display(Name = "Comments")]
        [DisplayFormat(NullDisplayText = "No comments for this Ticket")]
        public ICollection<Comment> Comments { get; set; }

        public bool Assigned { get; set; }
        public List<FileDetail> FileDetails { get; set; }
    }
    public enum Status
    {
        Queue,
        Progress,
        Completed
    }
    public enum Priority
    {
        Low,
        Medium,
        High
    }
}
