using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BugTrackerService.Models;
using BugTrackerService.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BugTrackerService.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> User { get; set; }
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
    }
}
