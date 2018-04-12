using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrackerService.Data;
using BugTrackerService.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BugTrackerService.Models.TicketViewModels;
using System.IO;
using BugTrackerService.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using BugTrackerService.Extensions;

namespace BugTrackerService.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ApplicationDbContext context, 
            UserManager<User> userManager, 
            IEmailSender emailSender, 
            IHostingEnvironment hostingEnvironment, 
            ILogger<TicketsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["OwnerSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PrioritySortParm"] = sortOrder == "Priority" ? "prio_desc" : "Priority";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "stat_desc" : "Status";
            ViewData["ProductSortParm"] = sortOrder == "Product" ? "prod_desc" : "Product";
            ViewData["AssignedSortParm"] = sortOrder == "Assigned" ? "ass_desc" : "Assigned";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            var tickets = from s in _context.Tickets.Include(c => c.Owner).Include(e => e.Employee).Include(p => p.Product)
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(d => d.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    tickets = tickets.OrderByDescending(nd => nd.Owner.LastName);
                    break;
                case "Date":
                    tickets = tickets.OrderBy(d => d.UpdateDate);
                    break;
                case "date_desc":
                    tickets = tickets.OrderByDescending(dd => dd.UpdateDate);
                    break;
                case "prio_desc":
                    tickets = tickets.OrderByDescending(pd => pd.Priority);
                    break;
                case "stat_desc":
                    tickets = tickets.OrderByDescending(sd => sd.Status);
                    break;
                case "prod_desc":
                    tickets = tickets.OrderByDescending(prd => prd.Product);
                    break;
                case "ass_desc":
                    tickets = tickets.OrderByDescending(assd => assd.Employee);
                    break;
                case "Priority":
                    tickets = tickets.OrderBy(p => p.Priority);
                    break;
                case "Assigned":
                    tickets = tickets.OrderBy(ass => ass.Employee);
                    break;
                case "Product":
                    tickets = tickets.OrderBy(pr => pr.Product);
                    break;
                case "Status":
                    tickets = tickets.OrderBy(st => st.Status);
                    break;
                default:
                    tickets = tickets.OrderBy(s => s.UpdateDate);
                    break;
            }
            return View(await tickets.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            Product[] products = _context.Products.ToArray();
            TicketCreateEditViewModel model = new TicketCreateEditViewModel()
            {
                Ticket = new Ticket(),
                Products = products.Select(x => new SelectListItem { Value = x.ProductId.ToString(), Text = x.Name })
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketCreateEditViewModel ticketModel)
        {
            var user = await GetCurrentUserAsync();
            var ticket = ticketModel.Ticket;
            ticket.ProductId = ticketModel.ProductId;
            ticket.Product = await _context.Products.SingleAsync(p => p.ProductId.Equals(ticket.ProductId));
            ticket.Status = Status.Queue;
            ticket.CreateDate = DateTime.Now;
            ticket.UpdateDate = DateTime.Now;
            ticket.Priority = Priority.Medium;
            ticket.OwnerId = user.Id;
            ticket.Owner = await _context.Users.SingleAsync(u => u.Id.Equals(ticket.OwnerId));
            
            await _userManager.AddToRoleAsync(user, "Owner");

            {
                List<FileDetail> fileDetails = await FileUploadHelperExtensions.UploadFileAsync(_hostingEnvironment,
                    _context, 
                    ticket.TicketId,
                    Request.Form.Files);
                ticket.FileDetails = fileDetails;
            }
            if (ModelState.IsValid)
            {
                
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketModel);
        }

        [Authorize(Policy = "RequireOwnerOrHigher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.Include(u => u.Owner)
                .Include(e => e.Employee)
                .Include(m => m.Product)
                .Include(f=>f.FileDetails)
                .SingleOrDefaultAsync(m => m.TicketId == id);
            Product[] products = _context.Products.ToArray();
            User[] users = _context.Users.Where(u => u.WorkerCardNumber != null).ToArray();
            var model = new TicketCreateEditViewModel()
            {
                Ticket = ticket,
                Products = products.Select(x => new SelectListItem { Value = x.ProductId.ToString(), Text = x.Name, Selected = true })
                ,
                Users = users.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName, Selected = true })
            };
            if (ticket == null)
            {
                return NotFound();
            }
            return View(model);
        }


        [Authorize(Policy = "RequireOwnerOrHigher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketCreateEditViewModel ticketModel)
        {
            var ticket = ticketModel.Ticket;
            if (id != ticket.TicketId)
            {
                return NotFound();
            }
            var oldTicket = await _context.Tickets.SingleOrDefaultAsync(t => t.TicketId == ticket.TicketId);
            oldTicket.Title = ticket.Title;
            oldTicket.Description = ticket.Description;
            oldTicket.Priority = ticket.Priority;
            oldTicket.Status = ticket.Status;
            oldTicket.UpdateDate = DateTime.Now;
            oldTicket.Product = ticket.Product;
            if (ticketModel.Users != null)
            {
                if (ticket.Assigned)
                {
                    oldTicket.Assigned = true;
                    var user = await GetCurrentUserAsync();
                    oldTicket.EmployeeId = user.Id;
                    oldTicket.Employee = await _context.Users.FirstAsync(u => u.Id.Equals(oldTicket.EmployeeId));
                    await _userManager.AddToRoleAsync(user, "Assigned");
                }
                else if (User.IsInRole("Admin"))
                {
                    oldTicket.Assigned = true;
                    oldTicket.EmployeeId = ticket.EmployeeId;
                    oldTicket.Employee = await _context.Users.FirstAsync(u => u.Id.Equals(oldTicket.EmployeeId));
                    await _userManager.AddToRoleAsync(oldTicket.Employee, "Assigned");
                }
            }
            else
            {
                oldTicket.Assigned = false;
                oldTicket.EmployeeId = null;
                oldTicket.Employee = null;
            }

            {
                List<FileDetail> fileDetails = await FileUploadHelperExtensions.UploadFileAsync(_hostingEnvironment,
                     _context,
                     ticket.TicketId,
                     Request.Form.Files);
                ticket.FileDetails = fileDetails;
            }
            if (ModelState.IsValid)
            {
                
                try
                {
                    var user = await _context.Users.FirstAsync(u => u.Id.Equals(oldTicket.OwnerId));
                    var callbackUrl = Url.EmailUpdateLink(oldTicket.TicketId, Request.Scheme);
                    await _emailSender.SendEmailUpdateAsync(user.Email, callbackUrl);
                    _context.Update(oldTicket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(oldTicket.TicketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticketModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug("Ticket not found");
                return NotFound();
            }

            var ticket = await _context.Tickets.Include(c => c.Owner).Include(e => e.Employee).Include(p => p.Product).Include(c => c.Comments).Include(f => f.FileDetails).FirstAsync(m => m.TicketId == id);
            var model = new TicketCommentViewModel() { Ticket = ticket, Comment = new Comment()};
            if (ticket == null)
            {
                _logger.LogDebug("Ticket not found");
                return NotFound();
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, TicketCommentViewModel model)
        {
            _logger.LogInformation("ID equals " + id);
            _logger.LogInformation("Model.Comment.Content = " + model.Comment.Content);
            var ticket = await _context.Tickets
                .FirstAsync(m => m.TicketId == id);
            _logger.LogInformation("My Tickets contain:" + ticket.TicketId + " " + ticket.Comments + " " + ticket.Title);
            if(ticket == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var comment = new Comment()
            {
                Content = model.Comment.Content,
                SendTime = DateTime.Now,
                TicketID = ticket.TicketId,
                Ticket = ticket,
                UserId = user.Id,
                User = user,
            };
            _logger.LogInformation("My new Comment contain: " + comment.Content + " " + comment.SendTime);
            ticket.Comments = new List<Comment>
            {
                comment
            };

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Comments.AddAsync(comment);
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(u => u.Owner).Include(e => e.Employee).Include(p => p.Product).SingleOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.Include(e => e.FileDetails).SingleOrDefaultAsync(m => m.TicketId == id);

            foreach (var item in ticket.FileDetails)
            {
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");
                var filePath = Path.Combine(uploads, item.Id + item.Extension);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    
                }
            }
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<JsonResult> DeleteFile(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return Json(new { result = false, message = "Failure!" });
            }
            try
            {
                Guid guid = new Guid(id);
                FileDetail fileDetail = _context.FileDetail.Find(guid);
                if (fileDetail == null)
                {
                    return Json(new { result = false, message = "Failure!" });
                }

                //Remove from database
                _context.FileDetail.Remove(fileDetail);
                await _context.SaveChangesAsync();

                //Delete file from the file system
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");
                var path = Path.Combine(uploads, fileDetail.Id + fileDetail.Extension);
                if (System.IO.File.Exists(uploads))
                {
                    System.IO.File.Delete(uploads);
                }
                return Json(new { result = true, message = "Success!" });
            }
            catch (IOException ex)
            {
                return Json(new { result = false, message = ex.Message });
            }

        }

        public async Task<IActionResult> Download(string fileName, string ticketId)
        {
            if (fileName == null)
                return Content("filename not present");

            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");
            var filePath = Path.Combine(uploads, fileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
                stream.Close();
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }

        private bool TicketExists(int id) => _context.Tickets.Any(e => e.TicketId == id);
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes() => new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
    }
}
