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
            TempData["Message"] = "Passagem adicionada com sucesso!";
            TempData["MessageType"] = "success";
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
            TempData["Message"] = "Passagem excluída com sucesso!";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Details), new { id = passage.ThemeId });

        }
        return NotFound();
    }

    public async Task<IActionResult> EditTheme(int id)
    {
        var theme = await _context.Themes.FindAsync(id);

        if (theme == null)
        {
            return NotFound(); 
        }
        return View(theme);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTheme(int id, Theme theme)
    {
        if (id != theme.Id)
        {
            return BadRequest("ID do tema não corresponde");
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Attach(theme).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Tema editado com sucesso!";
                TempData["MessageType"] = "success";
                // Redireciona de volta para a página de detalhes do tema
                return RedirectToAction(nameof(Details), new { id = theme.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Themes.Any(p => p.Id == theme.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        return View(theme);
    }
    
    // Exclui um tema (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTheme(int id)
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
    
    
    // GET: Exibir página de edição
    public async Task<IActionResult> EditPassage(int id)
    {
        var passage = await _context.Passages
            .Include(p => p.Book)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (passage == null)
        {
            return NotFound();
        }

        // Carrega os livros para o dropdown
        ViewBag.Books = await _context.Books.OrderBy(b => b.Id).ToListAsync();
        return View(passage);
    }

    // POST: Atualizar a passagem
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPassage(Passage passage)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Passages.Update(passage);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Passagem editada com sucesso!";
                TempData["MessageType"] = "success";
                // Redireciona de volta para a página de detalhes do tema
                return RedirectToAction(nameof(Details), new { id = passage.ThemeId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Passages.Any(p => p.Id == passage.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        // Caso haja erro, recarrega a página com os dados necessários
        ViewBag.Books = await _context.Books.OrderBy(b => b.Id).ToListAsync();
        return View(passage);
    }

}
