using BugTrackerService.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BugTrackerService.Models.TicketViewModels
{
    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }
        public IFormFile File { get; set; }
        public int ProductId { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
    }
}
