using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestPapalotes.Data;
using TestPapalotes.Models;

namespace TestPapalotes.Controllers
{
    // Requerimiento admin #2: mantenimiento (CRUD) de Profesores.
    public class ProfesoresController : Controller
    {
        private readonly ControlEscolarContext _db;
        public ProfesoresController(ControlEscolarContext db) => _db = db;

        public async Task<IActionResult> Index() =>
            View(await _db.Profesores.ToListAsync());

        public IActionResult Create() => View(new Profesor());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Profesor profesor)
        {
            if (!ModelState.IsValid) return View(profesor);
            _db.Add(profesor);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var profesor = await _db.Profesores.FindAsync(id);
            return profesor == null ? NotFound() : View(profesor);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profesor profesor)
        {
            if (id != profesor.Id) return NotFound();
            if (!ModelState.IsValid) return View(profesor);
            _db.Update(profesor);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var profesor = await _db.Profesores.FindAsync(id);
            return profesor == null ? NotFound() : View(profesor);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profesor = await _db.Profesores.FindAsync(id);
            if (profesor != null) _db.Profesores.Remove(profesor);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
