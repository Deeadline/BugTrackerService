using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BugTrackerService.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BugTrackerService.Data
{
    public class BugTrackerServiceContext : IdentityDbContext<User>
    {
        public BugTrackerServiceContext()
        {
        }

        public BugTrackerServiceContext(DbContextOptions<BugTrackerServiceContext> options) : base(options) { }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            modelBuilder.Entity<Ticket>().ToTable("Tickets");
            modelBuilder.Entity<User>().HasMany(t => t.EmployeeTickets).WithOne(t => t.Employee);
            modelBuilder.Entity<User>().HasMany(t => t.OwnerTickets).WithOne(t => t.Owner);
            modelBuilder.Entity<Ticket>().HasOne(u => u.Owner).WithMany(u => u.OwnerTickets);
            modelBuilder.Entity<Ticket>().HasOne(u => u.Employee).WithMany(u => u.EmployeeTickets);
        }


        public DbSet<BugTrackerService.Data.Models.User> User { get; set; }
    }
}
