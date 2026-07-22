using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestPapalotes.Data;
using TestPapalotes.Models;

namespace TestPapalotes.Controllers
{
    // Requerimiento admin #2: mantenimiento (CRUD) de Alumnos, incluyendo el grupo al que pertenecen.
    public class AlumnosController : Controller
    {
        private readonly ControlEscolarContext _db;
        public AlumnosController(ControlEscolarContext db) => _db = db;

        private void CargarGrupos(int? seleccionado = null) =>
            ViewBag.Grupos = new SelectList(_db.Grupos.OrderBy(g => g.Clave), "Id", "Clave", seleccionado);

        public async Task<IActionResult> Index() =>
            View(await _db.Alumnos.Include(a => a.Grupo).ToListAsync());

        public IActionResult Create()
        {
            CargarGrupos();
            return View(new Alumno());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Alumno alumno)
        {
            // Validación: la boleta es única, no puede repetirse entre alumnos.
            if (await _db.Alumnos.AnyAsync(a => a.Boleta == alumno.Boleta))
                ModelState.AddModelError("Boleta", "Ya existe un alumno con esa boleta.");

            if (!ModelState.IsValid) { CargarGrupos(alumno.GrupoId); return View(alumno); }
            _db.Add(alumno);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var alumno = await _db.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();
            CargarGrupos(alumno.GrupoId);
            return View(alumno);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Alumno alumno)
        {
            if (id != alumno.Id) return NotFound();

            // Validación: la boleta no debe repetirse con OTRO alumno distinto al que se edita.
            if (await _db.Alumnos.AnyAsync(a => a.Boleta == alumno.Boleta && a.Id != alumno.Id))
                ModelState.AddModelError("Boleta", "Ya existe un alumno con esa boleta.");

            if (!ModelState.IsValid) { CargarGrupos(alumno.GrupoId); return View(alumno); }
            _db.Update(alumno);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var alumno = await _db.Alumnos.Include(a => a.Grupo).FirstOrDefaultAsync(a => a.Id == id);
            return alumno == null ? NotFound() : View(alumno);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alumno = await _db.Alumnos.FindAsync(id);
            if (alumno != null)
            {
                // Quita primero sus inscripciones (no se borran en cascada por el lado del alumno).
                var inscripciones = _db.Inscripciones.Where(i => i.AlumnoId == id);
                _db.Inscripciones.RemoveRange(inscripciones);
                _db.Alumnos.Remove(alumno);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
