using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Services
{
    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }

        public int PrimaryPort { get; set; }

        public string UsernameEmail { get; set; }

        public string UsernamePassword { get; set; }

        public string FromEmail { get; set; }

        public string ToEmail { get; set; }

        public string CcEmail { get; set; }
    }
}
