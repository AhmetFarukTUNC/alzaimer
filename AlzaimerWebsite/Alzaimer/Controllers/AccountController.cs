using Microsoft.AspNetCore.Mvc;
using AlzheimerApp.Data;
using AlzheimerApp.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AlzheimerApp.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AlzheimerContext _context;

        public AccountController(AlzheimerContext context)
        {
            _context = context;
        }

        // Register sayfası
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Username already exists";
                return View();
            }

            var user = new User
            {
                Username = username,
                Password = password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Oturum aç
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetInt32("UserId", user.Id); // int olarak kaydediyoruz

            return RedirectToAction("Index", "Home");
        }

        // Login sayfası
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            // Oturum aç
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetInt32("UserId", user.Id); // int olarak kaydediyoruz

            return RedirectToAction("Index", "Home");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("UserId"); // UserId session’dan da kaldır
            return RedirectToAction("Login");
        }
    }
}
