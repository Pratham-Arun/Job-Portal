using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace JobPortal.ViewModels
{
    // ViewModels/RegisterViewModel.cs
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        // For applicants
        public List<int>? SelectedSkills { get; set; }

        // REMOVE THESE PROPERTIES
        // They don't belong in registration
    }

}
