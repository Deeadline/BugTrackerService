using BugTrackerService.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BugTrackerService.Data
{
    public class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            //adding customs roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { "Admin", "Employee", "User" };
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
            var _user = await UserManager.FindByEmailAsync("admin@email.com");
            if (_user == null)
            {
                var poweruser = new User
                {
                    FirstName = "Mikołaj",
                    LastName = "Szymański",
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com",
                    CompanyName = "Administracja",
                    PhoneNumber = "00-00",
                    WorkerCardNumber = "0"
                };

                string userPassword = "@dm!n!str@t0r";
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
