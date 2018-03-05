using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Models
{
    public class TicketEmployee
    {
        public int? EmployeeID { get; set; }
        public int? TicketID { get; set; }
        public EmployeeModel Employee { get; set; }
        public TicketModel Ticket { get; set; }
    }
}
