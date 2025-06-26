using JobPortal.Models;
using JobPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JobPortal.Data;
using JobPortal.Models;
using JobPortal.ViewModels;

namespace JobPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserRole", user.Role);
                    HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");

                    return user.Role switch
                    {
                        "admin" => RedirectToAction("Dashboard", "Admin"),
                        "employer" => RedirectToAction("Dashboard", "Employer"),
                        "applicant" => RedirectToAction("Dashboard", "Applicant"),
                        _ => RedirectToAction("Index", "Home")
                    };
                }

                ModelState.AddModelError("", "Invalid email or password");
            }

            return View(model);
        }

        public async Task<IActionResult> Register(string role = "applicant")
        {
            ViewBag.Role = role;
            ViewBag.Skills = await _context.Skills.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    ViewBag.Skills = await _context.Skills.ToListAsync();
                    return View(model);
                }

                var user = new User
                {
                    Email = model.Email,
                    Password = model.Password, // In production, hash this!
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role
                };

                if (model.Role == "applicant" && model.SelectedSkills != null)
                {
                    user.Skills = JsonConvert.SerializeObject(model.SelectedSkills);
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            ViewBag.Skills = await _context.Skills.ToListAsync();
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
