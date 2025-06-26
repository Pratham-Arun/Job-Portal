using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Services;
using JobPortal.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace JobPortal.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SkillMatchingService _skillMatchingService;

        public ApplicantController(
            ApplicationDbContext context,
            SkillMatchingService skillMatchingService)
        {
            _context = context;
            _skillMatchingService = skillMatchingService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var applications = await _context.Applications
                .Include(a => a.Job)
                .ThenInclude(j => j.Employer)
                .Where(a => a.ApplicantId == userId)
                .ToListAsync();

            return View(applications);
        }

        public async Task<IActionResult> Jobs()
        {
            var jobs = await _context.Jobs
                .Include(j => j.Employer)
                .ToListAsync();

            return View(jobs);
        }

        public async Task<IActionResult> Apply(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            // Check if job exists
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                TempData["Error"] = "The job you are trying to apply for does not exist.";
                return RedirectToAction("Jobs");
            }

            // Check if user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Your user account does not exist.";
                return RedirectToAction("Login", "Account");
            }

            // Prevent duplicate applications
            var existing = await _context.Applications
                .FirstOrDefaultAsync(a => a.JobId == id && a.ApplicantId == userId);
            if (existing != null)
            {
                TempData["Error"] = "You have already applied for this job.";
                return RedirectToAction("Jobs");
            }

            var application = new Application
            {
                JobId = id,
                ApplicantId = userId.Value,
                Status = "Pending",
                AppliedAt = DateTime.Now
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            TempData["Success"] = "You have successfully applied for the job.";
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var skills = await _context.Skills.ToListAsync();
            var selectedSkills = _skillMatchingService.GetSkillIds(user.Skills);

            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                Degree = user.Degree ?? "",
                Course = user.Course ?? "",
                SelectedSkills = selectedSkills
            };
            ViewBag.Skills = skills;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Degree = model.Degree;
            user.Course = model.Course;
            user.Skills = JsonConvert.SerializeObject(model.SelectedSkills);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Dashboard");
        }
    }
}
