using Microsoft.AspNetCore.Mvc;
using MvcLibrary.Data;
using System.Linq;

namespace MvcLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BookController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Action to list books
        public IActionResult Books()
        {
            var books = _dbContext.Books.ToList();
            return View(books);
        }

        // Other actions related to books can be added here
    }
}
