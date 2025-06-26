using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.ViewModels
{
    // ViewModels/EditProfileViewModel.cs
    public class EditProfileViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Degree { get; set; } = string.Empty;

        [Required]
        public string Course { get; set; } = string.Empty;

        public List<int> SelectedSkills { get; set; } = new List<int>();
    }


}
