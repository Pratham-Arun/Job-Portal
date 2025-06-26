using JobPortal.Models;
using Microsoft.EntityFrameworkCore;
using JobPortal.Models;

namespace JobPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Employer)
                .WithMany(u => u.Jobs)
                .HasForeignKey(j => j.EmployerId);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Applicant)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.ApplicantId);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Skills
            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "C#", Description = "C# Programming Language" },
                new Skill { Id = 2, Name = "ASP.NET", Description = "ASP.NET Framework" },
                new Skill { Id = 3, Name = "JavaScript", Description = "JavaScript Programming" },
                new Skill { Id = 4, Name = "SQL", Description = "SQL Database Management" },
                new Skill { Id = 5, Name = "React", Description = "React JavaScript Library" },
                new Skill { Id = 6, Name = "Python", Description = "Python Programming Language" },
                new Skill { Id = 7, Name = "Java", Description = "Java Programming Language" },
                new Skill { Id = 8, Name = "HTML/CSS", Description = "Web Markup and Styling" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "admin@resumescreening.com", Password = "admin123", Role = "admin", FirstName = "Admin", LastName = "User" },
                new User { Id = 2, Email = "employer@techcorp.com", Password = "employer123", Role = "employer", FirstName = "Tech", LastName = "Corp" },
                new User { Id = 3, Email = "applicant1@example.com", Password = "password123", Role = "applicant", FirstName = "John", LastName = "Doe", Skills = "[1,2,4]" },
                new User { Id = 4, Email = "applicant2@example.com", Password = "password123", Role = "applicant", FirstName = "Jane", LastName = "Smith", Skills = "[3,5,8]" },
                new User { Id = 5, Email = "applicant3@example.com", Password = "password123", Role = "applicant", FirstName = "Bob", LastName = "Johnson", Skills = "[1,3,4,6]" }
            );
        }
    }
}
