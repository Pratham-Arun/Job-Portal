using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data;
using JobPortal.Models;

namespace JobPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "admin") return RedirectToAction("Login", "Account");

            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalJobs = await _context.Jobs.CountAsync();
            ViewBag.TotalApplications = await _context.Applications.CountAsync();
            ViewBag.TotalSkills = await _context.Skills.CountAsync();

            return View();
        }

        public async Task<IActionResult> Users()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "admin") return RedirectToAction("Login", "Account");

            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Skills()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "admin") return RedirectToAction("Login", "Account");

            var skills = await _context.Skills.ToListAsync();
            return View(skills);
        }

        [HttpPost]
        public async Task<IActionResult> AddSkill(string name, string? description)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "admin") return RedirectToAction("Login", "Account");

            if (!string.IsNullOrEmpty(name))
            {
                // Check if skill already exists
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());

                if (existingSkill != null)
                {
                    TempData["Error"] = "A skill with this name already exists.";
                }
                else
                {
                    var skill = new Skill
                    {
                        Name = name.Trim(),
                        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim()
                    };

                    _context.Skills.Add(skill);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Skill added successfully!";
                }
            }
            else
            {
                TempData["Error"] = "Skill name is required.";
            }

            return RedirectToAction("Skills");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "admin") return RedirectToAction("Login", "Account");

            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                try
                {
                    _context.Skills.Remove(skill);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Skill deleted successfully!";
                }
                catch (Exception)
                {
                    TempData["Error"] = "Cannot delete skill. It may be in use by existing users or jobs.";
                }
            }
            else
            {
                TempData["Error"] = "Skill not found.";
            }

            return RedirectToAction("Skills");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "admin") return RedirectToAction("Login", "Account");

            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == id)
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction("Users");
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                try
                {
                    // Remove related applications first
                    var applications = await _context.Applications
                        .Where(a => a.ApplicantId == id)
                        .ToListAsync();
                    _context.Applications.RemoveRange(applications);

                    // Remove related jobs (for employers)
                    var jobs = await _context.Jobs
                        .Where(j => j.EmployerId == id)
                        .ToListAsync();
                    foreach (var job in jobs)
                    {
                        var jobApplications = await _context.Applications
                            .Where(a => a.JobId == job.Id)
                            .ToListAsync();
                        _context.Applications.RemoveRange(jobApplications);
                    }
                    _context.Jobs.RemoveRange(jobs);

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "User deleted successfully!";
                }
                catch (Exception)
                {
                    TempData["Error"] = "Error deleting user. Please try again.";
                }
            }
            else
            {
                TempData["Error"] = "User not found.";
            }

            return RedirectToAction("Users");
        }
    }
}