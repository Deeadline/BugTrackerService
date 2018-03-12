using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerService.Controllers
{
    public class ApplicationBaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}