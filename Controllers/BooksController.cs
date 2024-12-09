using HolyStudy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HolyStudy.Models;

namespace HolyStudy.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retorna a lista de livros
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books.OrderBy(b => b.Name).ToListAsync();
            return Json(books);
        }
    }
}
