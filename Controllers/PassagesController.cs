using HolyStudy.Data;
using HolyStudy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HolyStudy.Controllers;

public class PassagesController : Controller
{
    private readonly ApplicationDbContext _context;

    public PassagesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Adicionar uma nova passagem (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Passage passage)
    {
        if (ModelState.IsValid)
        {
            _context.Passages.Add(passage);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Passagem adicionada com sucesso!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Details", "Themes", new { id = passage.ThemeId });
        }

        TempData["Message"] = "Erro ao adicionar a passagem.";
        TempData["MessageType"] = "danger";
        return RedirectToAction("Details", "Themes", new { id = passage.ThemeId });
    }

    // Editar uma passagem (GET)
    public async Task<IActionResult> Edit(int id)
    {
        var passage = await _context.Passages
            .Include(p => p.Book)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (passage == null)
        {
            return NotFound();
        }

        ViewBag.Books = await _context.Books.OrderBy(b => b.Id).ToListAsync();
        return View(passage);
    }

    // Editar uma passagem (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Passage passage)
    {
        if (id != passage.Id)
        {
            return BadRequest("ID da passagem não corresponde.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Passages.Update(passage);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Passagem atualizada com sucesso!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Details", "Themes", new { id = passage.ThemeId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Passages.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        ViewBag.Books = await _context.Books.OrderBy(b => b.Id).ToListAsync();
        return View(passage);
    }

    // Excluir passagem (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var passage = await _context.Passages.FindAsync(id);

        if (passage != null)
        {
            _context.Passages.Remove(passage);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Passagem excluída com sucesso!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Details", "Themes", new { id = passage.ThemeId });
        }

        return NotFound();
    }
}
