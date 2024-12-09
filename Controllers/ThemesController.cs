using HolyStudy.Data;
using HolyStudy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HolyStudy.Controllers;

public class ThemesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ThemesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Página inicial
    public async Task<IActionResult> Index()
    {
        var favoriteThemes = await _context.Themes
            .OrderBy(t => t.Name)
            .Take(6)
            .ToListAsync();

        // Buscar todos os temas para o dropdown
        ViewBag.AllThemes = await _context.Themes
            .OrderBy(t => t.Name)
            .ToListAsync();

        return View(favoriteThemes);
    }

    // Página de conteúdo do tema
    public async Task<IActionResult> Details(int id)
    {
        var theme = await _context.Themes
            .Include(t => t.Passages)
            .ThenInclude(p => p.Book)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (theme == null)
        {
            return NotFound();
        }

        ViewBag.Books = await _context.Books.OrderBy(b => b.Id).ToListAsync();
        return View(theme);
    }


// Página de criação de tema (GET)
    public IActionResult Create()
    {
        // Prepara os livros para exibição no dropdown
        ViewBag.Books = _context.Books.OrderBy(b => b.Name).ToList();
        return View(new Theme());
    }

// Salva um novo tema (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Theme theme)
    {
        if (ModelState.IsValid)
        {
            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();

            // Prepara os livros para exibição no dropdown
            ViewBag.Books = _context.Books.OrderBy(b => b.Id).ToList();

            // Retorna a mesma view com o modelo atualizado
            return View(theme);
        }

        // Caso a validação falhe, recarrega a página
        ViewBag.Books = _context.Books.OrderBy(b => b.Id).ToList();
        return View(theme);
    }

    // Adiciona uma passagem ao tema (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPassage(Passage passage)
    {
        if (ModelState.IsValid)
        {
            _context.Passages.Add(passage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = passage.ThemeId });
        }

        // Busca o tema e os dados necessários para recarregar a view
        var theme = await _context.Themes
            .Include(t => t.Passages)
            .ThenInclude(p => p.Book)
            .FirstOrDefaultAsync(t => t.Id == passage.ThemeId);

        if (theme == null)
        {
            return NotFound();
        }

        ViewBag.Books = await _context.Books.OrderBy(b => b.Name).ToListAsync();
        return View("Details", theme);
    }


    // Exclui uma passagem (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePassage(int id)
    {
        var passage = await _context.Passages.FindAsync(id);
        if (passage != null)
        {
            _context.Passages.Remove(passage);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id = passage.ThemeId });
    }
}
