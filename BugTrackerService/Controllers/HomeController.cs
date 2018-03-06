using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BugTrackerService.Data.Models;

namespace BugTrackerService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Report()
        {
            ViewData["Message"] = "Report page. ";

            return View();
        }

        public IActionResult Dashboard()
        {
            ViewData["Message"] = "Dashboard page. ";
            return View();
        }

        public IActionResult Profile()
        {
            ViewData["Message"] = "Profile page. ";
            return View();
        }

        public IActionResult Register()
        {
            ViewData["Message"] = "Register page. ";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
