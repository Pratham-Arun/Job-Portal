using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    public class JobPostViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public List<int> RequiredSkills { get; set; } = new List<int>();
    }
}   