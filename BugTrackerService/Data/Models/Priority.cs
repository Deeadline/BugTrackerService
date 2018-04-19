using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Data.Models
{
    public class Priority
    {
        [Key]
        public int PriorityId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
    }
}
