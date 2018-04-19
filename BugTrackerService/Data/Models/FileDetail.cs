using System;

namespace BugTrackerService.Data.Models
{
    public class FileDetail
    {
        public Guid Id { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
