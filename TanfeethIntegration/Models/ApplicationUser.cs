using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

    }
}
