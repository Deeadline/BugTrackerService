using BugTrackerService.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Models.TicketViewModels
{
    public class TicketCommentViewModel
    {
        public Ticket Ticket { get; set; }
        public Comment Comment { get; set; }
    }
}
