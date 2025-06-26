using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string SkillsRequired { get; set; } = string.Empty; // JSON string

        public int EmployerId { get; set; }
        public virtual User Employer { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}