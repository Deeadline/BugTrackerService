﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerService.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            OwnerTickets = new List<Ticket>();
            EmployeeTickets = new List<Ticket>();
        }
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        public string WorkerCardNumber { get; set; }
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
        [DisplayFormat(NullDisplayText = "No tickets")]
        [Display(Name ="Owner Tickets")]
        public virtual ICollection<Ticket> OwnerTickets { get; set; }
        [DisplayFormat(NullDisplayText = "No tickets")]
        [Display(Name = "Employee Tickets")]
        public virtual ICollection<Ticket> EmployeeTickets { get; set; }

    }
}
