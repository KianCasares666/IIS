using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InstructorInformationManagement.Models;
using InstructorInformationManagement.Data;

namespace InstructorInformationManagement.Controllers
{
    [Authorize] // Protects the controller so only authenticated users can access records
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Instructor
        public async Task<IActionResult> Index()
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name);

            // Only fetch instructors belonging to the currently logged-in user
            var instructors = await _context.Instructors
                .Where(i => i.UserEmail == currentUserEmail)
                .ToListAsync();

            return View(instructors);
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name);
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id && m.UserEmail == currentUserEmail);

            if (instructor == null) return NotFound();

            return View(instructor);
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InstructorIdNumber,FullName,Address,Gender,Age,Email,Majorship,Designation,BachelorDegree")] Instructor instructor)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name);
            instructor.UserEmail = currentUserEmail;

            // Remove ModelState validation error for UserEmail since it's assigned automatically
            ModelState.Remove(nameof(Instructor.UserEmail));

            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name);
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id && m.UserEmail == currentUserEmail);

            if (instructor == null) return NotFound();

            return View(instructor);
        }

        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InstructorIdNumber,FullName,Address,Gender,Age,Email,Majorship,Designation,BachelorDegree")] Instructor instructor)
        {
            if (id != instructor.Id) return NotFound();

            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name);

            // Verify that the record being edited belongs to the current user
            var existingInstructor = await _context.Instructors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id && m.UserEmail == currentUserEmail);

            if (existingInstructor == null) return NotFound();

            instructor.UserEmail = currentUserEmail;
            ModelState.Remove(nameof(Instructor.UserEmail));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name);
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id && m.UserEmail == currentUserEmail);

            if (instructor == null) return NotFound();

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name);
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id && m.UserEmail == currentUserEmail);

            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name);
            return _context.Instructors.Any(e => e.Id == id && e.UserEmail == currentUserEmail);
        }
    }
}