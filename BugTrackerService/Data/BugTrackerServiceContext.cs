using BugTrackerService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerService.Data
{
    public class BugTrackerServiceContext : DbContext
    {
        public BugTrackerServiceContext(DbContextOptions<BugTrackerServiceContext> options) : base(options) { }

        public DbSet<UserModel> Persons { get; set; }
        public DbSet<TicketModel> Tickets { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<TicketModel>().ToTable("Tickets");
            modelBuilder.Entity<EmployeeModel>().ToTable("Employees");
            modelBuilder.Entity<UserModel>().HasMany(t => t.Tickets)
                .WithOne(t => t.User);
            modelBuilder.Entity<TicketModel>().HasOne(u => u.User)
                .WithMany(u => u.Tickets);
        }
    }
}
