using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcLibrary.Models;

namespace MvcLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<Adherent> _userManager;

        public HomeController(UserManager<Adherent> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User); // Get the user ID
                var user = await _userManager.FindByIdAsync(userId);
                ViewData["FullName"] = user.FullName; // Pass the full name to the view
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Success()
        {
            // Your logic here, if any
            return View();
        }
        public IActionResult Error()
        {
            // Your logic here, if any
            return View();
        }
    }
}
