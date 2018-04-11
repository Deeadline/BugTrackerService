using BugTrackerService.Data.Models;

namespace BugTrackerService.Models.TicketViewModels
{
    public class TicketCommentViewModel
    {
        public Ticket Ticket { get; set; }
        public Comment Comment { get; set; }
    }
}
