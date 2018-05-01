using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerService.Data.Models
{
    public class FileDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
