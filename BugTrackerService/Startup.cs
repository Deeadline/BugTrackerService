using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BugTrackerService.Data;
using BugTrackerService.Services;
using BugTrackerService.Data.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BugTrackerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LinuxConnection")));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireOwnerOrHigher", policy => policy.RequireRole("Admin", "Owner", "Employee"));
            });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/myapp-{Date}.txt", LogLevel.Information);
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            CreateRoles(serviceProvider).Wait();
        }
        private async Task CreateRoles(IServiceProvider serviceProvider)
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
