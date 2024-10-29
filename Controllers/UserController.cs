using Microsoft.AspNetCore.Mvc;
using TaskMIS.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskMIS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using System.Net;

namespace TaskMIS.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;

        }

        // Gets the Registration Page
        public IActionResult Register() => View();


        // Saves the data for Registration by the New User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                // Validates the data and displays errors if all the requirements are not met
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); 
                }
                return View(user);
            }

            
            if (ModelState.IsValid)
            {
                // HashPassword function is called.
                user.Password = HashPassword(user.Password);  
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // Displays the Login Page
        public IActionResult Login() => View();

        // Logs into the main page
        [HttpPost]
        
        public async Task<IActionResult> Login(User us)
        {
            var hashedPassword = HashPassword(us.Password);
            var user = _db.Users.FirstOrDefault(u => u.Username == us.Username && u.Password == hashedPassword);
            if (user != null)
            {
                // Create claims 
                var claims = new List<Claim>
        {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                 new Claim(ClaimTypes.Name, user.Username) 
        };

                // Create claims identity to store identity information
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in the user in statelessness
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                // the cookie to lasts beyond the session
                    new AuthenticationProperties { IsPersistent = true }); 

                return RedirectToAction("Create", "DailyTask");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }
        // Hashes the password for security
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

     
    }
}
