using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestPapalotes.Data;
using TestPapalotes.Models;

namespace TestPapalotes.Controllers
{
    // Requerimiento admin #3: asignar profesor y alumnos (a través del grupo) a cada materia.
    public class AsignacionesController : Controller
    {
        private readonly ControlEscolarContext _db;
        public AsignacionesController(ControlEscolarContext db) => _db = db;


        private void CargarCombos(Asignacion? a = null)
        {
            ViewBag.Materias = new SelectList(_db.Materias.OrderBy(m => m.Nombre), "Id", "Nombre", a?.MateriaId);
            ViewBag.Grupos = new SelectList(_db.Grupos.OrderBy(g => g.Clave), "Id", "Clave", a?.GrupoId);
            ViewBag.Profesores = new SelectList(_db.Profesores.OrderBy(p => p.Nombre), "Id", "Nombre", a?.ProfesorId);
        }

        public async Task<IActionResult> Index() =>
            View(await _db.Asignaciones
                .Include(a => a.Materia).Include(a => a.Grupo).Include(a => a.Profesor)
                .ToListAsync());

        public IActionResult Create()
        {
            CargarCombos();
            return View(new Asignacion());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Asignacion asignacion)
        {
            if (!ModelState.IsValid) { CargarCombos(asignacion); return View(asignacion); }
            _db.Add(asignacion);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var asignacion = await _db.Asignaciones.FindAsync(id);
            if (asignacion == null) return NotFound();
            CargarCombos(asignacion);
            return View(asignacion);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Asignacion asignacion)
        {
            if (id != asignacion.Id) return NotFound();
            if (!ModelState.IsValid) { CargarCombos(asignacion); return View(asignacion); }

            var original = await _db.Asignaciones.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (original == null) return NotFound();

            // No se puede cambiar el grupo si ya hay alumnos inscritos en esta asignación:
            // esos alumnos pertenecen al grupo anterior. Primero hay que quitarlos (Desinscribir).
            if (original.GrupoId != asignacion.GrupoId)
            {
                bool tieneInscritos = await _db.Inscripciones.AnyAsync(i => i.AsignacionId == id);
                if (tieneInscritos)
                {
                    ModelState.AddModelError(string.Empty,
                        "No puedes cambiar el grupo mientras haya alumnos inscritos en esta asignación. " +
                        "Primero quita a los alumnos desde 'Detalle' y luego cambia el grupo.");
                    CargarCombos(asignacion);
                    return View(asignacion);
                }
            }

            _db.Update(asignacion);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Gestiona qué alumnos del grupo están inscritos en esta materia con este profesor.
        public async Task<IActionResult> Detalle(int id)
        {
            var asignacion = await _db.Asignaciones
                .Include(a => a.Materia).Include(a => a.Grupo).Include(a => a.Profesor)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (asignacion == null) return NotFound();

            // Todos los alumnos del grupo (candidatos) y cuáles ya están inscritos.
            ViewBag.Alumnos = await _db.Alumnos
                .Where(al => al.GrupoId == asignacion.GrupoId)
                .OrderBy(al => al.Nombre).ToListAsync();

            ViewBag.Inscritos = await _db.Inscripciones
                .Where(i => i.AsignacionId == id)
                .Select(i => i.AlumnoId)
                .ToListAsync();

            return View(asignacion);
        }

        // Asigna (inscribe) un alumno a la materia.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscribir(int asignacionId, int alumnoId)
        {
            bool yaExiste = await _db.Inscripciones
                .AnyAsync(i => i.AsignacionId == asignacionId && i.AlumnoId == alumnoId);

            if (!yaExiste)
            {
                _db.Inscripciones.Add(new Inscripcion { AsignacionId = asignacionId, AlumnoId = alumnoId });
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Detalle), new { id = asignacionId });
        }

        // Quita (elimina) la inscripción del alumno en la materia.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Desinscribir(int asignacionId, int alumnoId)
        {
            var inscripcion = await _db.Inscripciones
                .FirstOrDefaultAsync(i => i.AsignacionId == asignacionId && i.AlumnoId == alumnoId);

            if (inscripcion != null)
            {
                _db.Inscripciones.Remove(inscripcion);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Detalle), new { id = asignacionId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var asignacion = await _db.Asignaciones
                .Include(a => a.Materia).Include(a => a.Grupo).Include(a => a.Profesor)
                .FirstOrDefaultAsync(a => a.Id == id);
            return asignacion == null ? NotFound() : View(asignacion);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asignacion = await _db.Asignaciones.FindAsync(id);
            if (asignacion != null) _db.Asignaciones.Remove(asignacion);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
