using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AlzheimerApp.Data;
using AlzheimerApp.Data.Models;

namespace AlzheimerApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly AlzheimerContext _context;

        public HomeController(AlzheimerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        public IActionResult About() => View();
        public IActionResult Privacy() => View();

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(string Name, string Email, string Message)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Message))
            {
                ViewBag.Error = "Please fill all fields correctly.";
                return View();
            }

            var contactMessage = new ContactMessage
            {
                Name = Name,
                Email = Email,
                Message = Message
            };

            _context.ContactMessages.Add(contactMessage);
            _context.SaveChanges();

            ViewBag.Success = "Your message has been sent successfully!";
            return View();
        }
    }
}
