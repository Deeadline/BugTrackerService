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

namespace BugTrackerService.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TicketsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //// GET: Tickets
        //public async Task<IActionResult> Index()
        //{
        //    var tickets = _context.Tickets.Include(c => c.Owner).Include(e => e.Employee).Include(p => p.Product);
        //    return View(await tickets.ToListAsync());
        //}

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
                tickets = tickets.Where(d => d.Description.Contains(searchString));
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

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.Include(c => c.Owner).Include(e=>e.Employee).Include(p=>p.Product).SingleOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
           Product[] products = _context.Products.ToArray();
            TicketViewModel model = new TicketViewModel()
            {
                Ticket = new Ticket(),
                Products = products.Select(x => new SelectListItem { Value = x.ProductId.ToString(), Text = x.Name })
            };
            return View(model);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModel ticketModel)
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
            if (ModelState.IsValid)
            {
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketModel);
        }

        [Authorize(Roles = "Employee, Admin")]
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.Include(u=>u.Owner).Include(e => e.Employee).Include(m => m.Product).SingleOrDefaultAsync(m => m.TicketId == id);
            Product[] products = _context.Products.ToArray();
            var model = new TicketViewModel() { Ticket = ticket, Products = products.Select(x => new SelectListItem { Value = x.ProductId.ToString(), Text = x.Name, Selected = true })};
            if (ticket == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Employee, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketViewModel ticketModel)
        {
            var ticket = ticketModel.Ticket;
            //ticket.UpdateDate = DateTime.Now;
            if (id != ticket.TicketId)
            {
                return NotFound();
            }
            var oldTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketModel.Ticket.TicketId);
            oldTicket.Title = ticketModel.Ticket.Title;
            oldTicket.Description = ticketModel.Ticket.Description;
            oldTicket.Priority = ticketModel.Ticket.Priority;
            oldTicket.Status = ticketModel.Ticket.Status;
            oldTicket.UpdateDate = DateTime.Now;
            if (ticketModel.Ticket.Assigned)
            {
                var user = await GetCurrentUserAsync();
                oldTicket.EmployeeId = user.Id;
                oldTicket.Employee = await _context.Users.SingleAsync(u => u.Id.Equals(oldTicket.EmployeeId));
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            var ticket = await _context.Tickets.SingleOrDefaultAsync(m => m.TicketId == id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
