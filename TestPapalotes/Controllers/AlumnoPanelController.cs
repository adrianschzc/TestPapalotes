using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestPapalotes.Data;
using TestPapalotes.Models;

namespace TestPapalotes.Controllers
{
    // Panel del alumno: mediante su boleta consulta las calificaciones de las
    // materias registradas en el grupo al que pertenece (solo lectura).
    public class AlumnoPanelController : Controller
    {
        private readonly ControlEscolarContext _db;
        public AlumnoPanelController(ControlEscolarContext db) => _db = db;

        public IActionResult Index() => View(new BoletaCalificacionesVM());

        [HttpPost]
        public async Task<IActionResult> Consultar(string boleta)
        {
            var alumno = await _db.Alumnos.Include(a => a.Grupo)
                .FirstOrDefaultAsync(a => a.Boleta == boleta);

            if (alumno == null)
            {
                ViewBag.Error = $"No se encontró ningún alumno con la boleta '{boleta}'.";
                return View(nameof(Index), new BoletaCalificacionesVM { Boleta = boleta });
            }

            // Solo las materias en las que el alumno está inscrito (asignadas por el admin).
            var asignaciones = await _db.Inscripciones
                .Where(i => i.AlumnoId == alumno.Id)
                .Include(i => i.Asignacion!).ThenInclude(a => a.Materia)
                .Include(i => i.Asignacion!).ThenInclude(a => a.Profesor)
                .Select(i => i.Asignacion!)
                .ToListAsync();

            var califs = await _db.Calificaciones
                .Where(c => c.AlumnoId == alumno.Id).ToListAsync();

            var vm = new BoletaCalificacionesVM
            {
                Boleta = boleta,
                AlumnoId = alumno.Id,
                AlumnoNombre = alumno.Nombre,
                Grupo = alumno.Grupo?.Clave ?? "(sin grupo)",
                Filas = asignaciones.Select(a => new CalificacionRow
                {
                    AsignacionId = a.Id,
                    Materia = a.Materia?.Nombre ?? "",
                    Profesor = a.Profesor?.Nombre ?? "",
                    Valor = califs.FirstOrDefault(c => c.AsignacionId == a.Id)?.Valor
                }).ToList()
            };
            return View(nameof(Index), vm);
        }
    }
}
