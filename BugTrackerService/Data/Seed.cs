using BugTrackerService.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerService.Data
{
    public static class Seed
    {
        public static void
        Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Priorities.Any())
            {
                context.Priorities.Add(new Priority() { Name = "Low", Class = "default" });
                context.Priorities.Add(new Priority() { Name = "Medium", Class = "medium" });
                context.Priorities.Add(new Priority() { Name = "High", Class = "reject" });
                context.SaveChanges();
            }
            if (!context.Statuses.Any())
            {
                context.Statuses.Add(new Status() { Name = "In Queue", Class = "default" });
                context.Statuses.Add(new Status() { Name = "In Progress", Class = "progr" });
                context.Statuses.Add(new Status() { Name = "In Testing", Class = "test" });
                context.Statuses.Add(new Status() { Name = "Completed", Class = "succ" });
                context.Statuses.Add(new Status() { Name = "Rejected", Class = "reject" });
                context.SaveChanges();
            }
            if (!context.Products.Any())
            {
                context.Products.Add(new Product() { Name = "Visual Studio" });
                context.Products.Add(new Product() { Name = "Excel 2016" });
                context.Products.Add(new Product() { Name = "Adobe After Effects" });
                context.Products.Add(new Product() { Name = "Discord" });
                context.SaveChanges();
            }
        }
    }
}
