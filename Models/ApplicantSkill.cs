using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models
{
    public class ApplicantSkill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SkillId { get; set; }

        [Range(1, 10)]
        public int ProficiencyLevel { get; set; }

        public int YearsOfExperience { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("SkillId")]
        public virtual Skill Skill { get; set; }
    }
}