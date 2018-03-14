using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Data.Models
{
    public class Ticket
    {
        private DateTime _createDate;
        private DateTime _updateDate;
        private Priority _priority;
        private int _id;

        [Key]
        [Display(Name="ID")]
        public int TicketId { get => _id; set => _id = value; }
        [Display(Name = "Created by")]
        public string OwnerId { get; set; }
        [Display(Name = "Assigned to")]
        [DisplayFormat(NullDisplayText = "No one is assigned to this ticket")]
        public string EmployeeId { get; set; }
        [Display(Name = "Product")]
        public int? ProductId { get; set; }
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
        public DateTime CreateDate { get => _createDate; set => _createDate = DateTime.Now; }
        [Display(Name = "Update Date")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get => _updateDate; set => _updateDate = DateTime.Now; }
        public User Owner { get; set; }
        public User Employee { get; set; }
        public Product Product { get; set; }
        [Display(Name = "Comments")]
        [DisplayFormat(NullDisplayText = "No comments for this Ticket")]
        public ICollection<Comment> Comments { get; set; }
    }
    public enum Status
    {
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
