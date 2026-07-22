using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestPapalotes.Data;
using TestPapalotes.Models;

namespace TestPapalotes.Controllers
{
    // Requerimiento admin #2: mantenimiento (CRUD) de Materias.
    public class MateriasController : Controller
    {
        private readonly ControlEscolarContext _db;
        public MateriasController(ControlEscolarContext db) => _db = db;

        public async Task<IActionResult> Index() =>
            View(await _db.Materias.ToListAsync());

        public IActionResult Create() => View(new Materia());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Materia materia)
        {
            if (!ModelState.IsValid) return View(materia);
            _db.Add(materia);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var materia = await _db.Materias.FindAsync(id);
            return materia == null ? NotFound() : View(materia);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Materia materia)
        {
            if (id != materia.Id) return NotFound();
            if (!ModelState.IsValid) return View(materia);
            _db.Update(materia);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var materia = await _db.Materias.FindAsync(id);
            return materia == null ? NotFound() : View(materia);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materia = await _db.Materias.FindAsync(id);
            if (materia != null) _db.Materias.Remove(materia);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
