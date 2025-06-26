using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JobPortal.Data;
using JobPortal.Models;
using JobPortal.ViewModels;

namespace JobPortal.Controllers
{
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == userId)
                .Include(j => j.Applications)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return View(jobs);
        }

        public async Task<IActionResult> PostJob()
        {
            ViewBag.Skills = await _context.Skills.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostJob(JobPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null) return RedirectToAction("Login", "Account");

                var job = new Job
                {
                    Title = model.Title,
                    Description = model.Description,
                    SkillsRequired = JsonConvert.SerializeObject(model.RequiredSkills),
                    EmployerId = userId.Value
                };

                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Job posted successfully!";
                return RedirectToAction("Dashboard");
            }

            ViewBag.Skills = await _context.Skills.ToListAsync();
            return View(model);
        }

        // Updated Applicants method in EmployerController.cs
        public async Task<IActionResult> Applicants(int jobId)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var job = await _context.Jobs
                    .Include(j => j.Applications)
                    .ThenInclude(a => a.Applicant)
                    .FirstOrDefaultAsync(j => j.Id == jobId && j.EmployerId == userId);

                if (job == null)
                {
                    TempData["Error"] = "Job not found or you don't have permission to view it.";
                    return RedirectToAction("Dashboard");
                }

                ViewBag.Job = job;

                // Return applications ordered by match score descending
                var applications = job.Applications.OrderByDescending(a => a.MatchScore).ToList();

                return View(applications);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging configured
                TempData["Error"] = "An error occurred while loading applicants.";
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, string status)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var application = await _context.Applications
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.Id == applicationId && a.Job.EmployerId == userId);

            if (application != null)
            {
                application.Status = status;
                application.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Application status updated to {status}";
            }
            else
            {
                TempData["Error"] = "Application not found or access denied.";
            }

            return RedirectToAction("Applicants", new { jobId = application?.JobId });
        }
    }
}