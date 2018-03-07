using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BugTrackerService.Models;
using BugTrackerService.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerService.Data
{
    public class BugTrackerServiceContext : IdentityDbContext<UserModel>
    {
        public BugTrackerServiceContext(DbContextOptions<BugTrackerServiceContext> options) : base(options) { }

        public DbSet<TicketModel> Tickets { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
