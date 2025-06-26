namespace JobPortal.Models
{
    public class Application
    {
        public int Id { get; set; }

        public int ApplicantId { get; set; }
        public virtual User Applicant { get; set; } = null!;

        public int JobId { get; set; }
        public virtual Job Job { get; set; } = null!;

        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected

        public double MatchScore { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}