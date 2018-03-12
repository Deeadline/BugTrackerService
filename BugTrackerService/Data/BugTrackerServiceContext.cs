using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BugTrackerService.Models;
using BugTrackerService.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BugTrackerService.Data
{
    public class BugTrackerServiceContext : IdentityDbContext<User>
    {
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
            modelBuilder.Entity<User>().HasMany(t => t.Tickets)
                .WithOne(t => t.User);
            modelBuilder.Entity<Ticket>().HasOne(u => u.User)
                .WithMany(u => u.Tickets);
        }
    }
}
