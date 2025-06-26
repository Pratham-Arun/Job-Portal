using JobPortal.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty; // admin, employer, applicant

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Skills { get; set; } // JSON string for applicants

        // NEW FIELDS
        public string? PhoneNumber { get; set; }
        public string? Degree { get; set; }
        public string? Course { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
