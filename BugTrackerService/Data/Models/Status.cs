using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Data.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
    }
}
