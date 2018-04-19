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
        public static async Task
        InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
            if (!context.Priorities.Any())
            {
                await context.Priorities.AddAsync(new Priority() { Name = "Low", Class = "default" });
                await context.Priorities.AddAsync(new Priority() { Name = "Medium", Class = "medium" });
                await context.Priorities.AddAsync(new Priority() { Name = "High", Class = "reject" });
                await context.SaveChangesAsync();
            }
            if (!context.Statuses.Any())
            {
                await context.Statuses.AddAsync(new Status() { Name = "In Queue", Class = "default" });
                await context.Statuses.AddAsync(new Status() { Name = "In Progress", Class = "progr" });
                await context.Statuses.AddAsync(new Status() { Name = "In Testing", Class = "test" });
                await context.Statuses.AddAsync(new Status() { Name = "Completed", Class = "succ" });
                await context.Statuses.AddAsync(new Status() { Name = "Rejected", Class = "reject" });
                await context.SaveChangesAsync();
            }
            if (!context.Products.Any())
            {
                await context.Products.AddAsync(new Product() { Name = "Visual Studio" });
                await context.Products.AddAsync(new Product() { Name = "Excel 2016" });
                await context.Products.AddAsync(new Product() { Name = "Adobe After Effects" });
                await context.Products.AddAsync(new Product() { Name = "Discord" });
                await context.SaveChangesAsync();
            }
        }
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            //adding customs roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { "Admin", "Employee", "User", "Owner", "Assigned" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }

            }
            // creating a super user who could maintain the web app
            var _user = await UserManager.FindByEmailAsync("admin@admin.com");
            if (_user == null)
            {
                var poweruser = new User
                {
                    FirstName = "Admini",
                    LastName = "Admin",
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com",
                    CompanyName = "Administracja",
                    PhoneNumber = "00-00",
                    WorkerCardNumber = "0",
                };

                string userPassword = "Adm!n!str@t0r";
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    // here we assign the new user the "Admin" role 
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }
    }
}
