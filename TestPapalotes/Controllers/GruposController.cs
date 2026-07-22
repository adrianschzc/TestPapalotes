using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestPapalotes.Data;
using TestPapalotes.Models;

namespace TestPapalotes.Controllers
{
    // Requerimiento admin #2: mantenimiento (CRUD) de Grupos.
    public class GruposController : Controller
    {
        private readonly ControlEscolarContext _db;
        public GruposController(ControlEscolarContext db) => _db = db;

        public async Task<IActionResult> Index() =>
            View(await _db.Grupos.ToListAsync());

        public IActionResult Create() => View(new Grupo());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grupo grupo)
        {
            // Validación: no permitir dos grupos con la misma clave.
            if (await _db.Grupos.AnyAsync(g => g.Clave == grupo.Clave))
                ModelState.AddModelError("Clave", "No puede existir dos grupos con la misma clave.");

            if (!ModelState.IsValid) return View(grupo);
            _db.Add(grupo);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var grupo = await _db.Grupos.FindAsync(id);
            return grupo == null ? NotFound() : View(grupo);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grupo grupo)
        {
            if (id != grupo.Id) return NotFound();

            // Validación: la clave no debe repetirse con OTRO grupo distinto al que se edita.
            if (await _db.Grupos.AnyAsync(g => g.Clave == grupo.Clave && g.Id != grupo.Id))
                ModelState.AddModelError("Clave", "No puede existir dos grupos con la misma clave.");

            if (!ModelState.IsValid) return View(grupo);
            _db.Update(grupo);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var grupo = await _db.Grupos.FindAsync(id);
            return grupo == null ? NotFound() : View(grupo);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupo = await _db.Grupos.FindAsync(id);
            if (grupo != null) _db.Grupos.Remove(grupo);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
