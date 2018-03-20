using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Data.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
}
