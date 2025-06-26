using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data;

namespace JobPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var jobs = await _context.Jobs
                .Where(j => j.IsActive)
                .Include(j => j.Employer)
                .OrderByDescending(j => j.CreatedAt)
                .Take(10)
                .ToListAsync();

            return View(jobs);
        }
    }
}