using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BugTrackerService.Models;

namespace BugTrackerService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Welcome on main page!";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
