using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Models.AccountViewModels
{
    public class UserEditViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string WorkerCardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
