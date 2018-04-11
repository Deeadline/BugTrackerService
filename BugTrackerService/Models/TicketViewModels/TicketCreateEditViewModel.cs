﻿using BugTrackerService.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BugTrackerService.Models.TicketViewModels
{
    public class TicketCreateEditViewModel
    {
        public Ticket Ticket { get; set; }
        public List<IFormFile> Files { get; set; }
        public FileDetail FilePath { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
