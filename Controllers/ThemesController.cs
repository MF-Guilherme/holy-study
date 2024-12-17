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
        var allThemes = await _context.Themes
            .OrderBy(t => t.Name)
            .ToListAsync();
        
        var favoriteThemes = await _context.Themes
            .Where(t => t.IsFavorite)
            .OrderBy(t => t.Name)
            .Take(8)
            .ToListAsync();
        
        ViewBag.FavoriteThemes = favoriteThemes;

        return View(allThemes);
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
            TempData["Message"] = "Tema criado com sucesso!";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Details), new { id = theme.Id });
        }

        return View(theme);
    }

    // GET: Exibir página de edição
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var theme = await _context.Themes.FindAsync(id);

        if (theme == null)
        {
            return NotFound();
        }

        return View(theme);
    }

    // POST: Atualizar tema
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Theme theme)
    {
        if (id != theme.Id)
        {
            return BadRequest("ID do tema não corresponde.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Themes.Update(theme);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Tema atualizado com sucesso!";
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Details), new { id = theme.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Themes.Any(t => t.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        return View(theme);
    }

    // Excluir tema (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var theme = await _context.Themes.FindAsync(id);

        if (theme != null)
        {
            _context.Themes.Remove(theme);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Tema excluído com sucesso!";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }
}
