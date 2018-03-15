﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrackerService.Data;
using BugTrackerService.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerService.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly BugTrackerServiceContext _context;
        private readonly UserManager<User> _userManager;

        public TicketsController(BugTrackerServiceContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = _context.Tickets.Include(c => c.Owner).Include(e => e.Employee);
            return View(await tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.Include(c => c.Owner).Include(e=>e.Employee).SingleOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            var products = _context.Products.ToList();
            ViewBag.ProductList = new SelectList(products, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,ProductId,Title,Description")] Ticket ticket)
        {
            var user = await GetCurrentUserAsync();
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
            return View(ticket);
        }

        [Authorize(Roles = "Employee, Admin")]
        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.Include(u=>u.Owner).Include(e => e.Employee).SingleOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Employee, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,ProductId,Title,Description,Status,Priority,Assigned")] Ticket ticket)
        {
            //ticket.UpdateDate = DateTime.Now;
            if (id != ticket.TicketId)
            {
                return NotFound();
            }
            var oldTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticket.TicketId);
            oldTicket.Title = ticket.Title;
            oldTicket.Description = ticket.Description;
            oldTicket.Priority = ticket.Priority;
            oldTicket.Status = ticket.Status;
            oldTicket.UpdateDate = DateTime.Now;
            if (ticket.Assigned)
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
            return View(oldTicket);
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
                .Include(u => u.Owner).Include(e => e.Employee).SingleOrDefaultAsync(m => m.TicketId == id);
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
