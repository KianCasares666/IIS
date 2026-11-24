using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using InstructorInformationManagement.Data;
using InstructorInformationManagement.Models;

namespace InstructorInformationManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                // Normalize email to lowercase for consistent checking
                var normalizedEmail = userAccount.Email.Trim().ToLower();

                // Check if email already exists
                var existingUser = await _context.UserAccounts
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(userAccount);
                }

                userAccount.Email = normalizedEmail; // Save as lowercase for security consistency
                _context.Add(userAccount);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction(nameof(Login));
            }
            return View(userAccount);
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and Password are required.");
                return View();
            }

            // Normalize input email to match the database securely
            var normalizedEmail = email.Trim().ToLower();

            var user = await _context.UserAccounts
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail && u.Password == password);

            if (user != null)
            {
                // Establish authenticated cookie session (from module)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email)
                };
                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                return RedirectToAction("Index", "Instructor");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Clear authentication cookie (from module)
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}