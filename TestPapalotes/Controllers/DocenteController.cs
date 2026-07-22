using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestPapalotes.Data;
using TestPapalotes.Models;

namespace TestPapalotes.Controllers
{
    // Panel del docente: mediante el número de boleta registra las calificaciones del alumno.
    public class DocenteController : Controller
    {
        private readonly ControlEscolarContext _db;
        public DocenteController(ControlEscolarContext db) => _db = db;

        public IActionResult Index() => View(new BoletaCalificacionesVM());

        // Busca al alumno por boleta y arma las filas (materias de su grupo + calificación actual).
        [HttpPost]
        public async Task<IActionResult> Buscar(string boleta)
        {
            var vm = await ConstruirVM(boleta);
            if (vm == null)
            {
                ViewBag.Error = $"No se encontró ningún alumno con la boleta '{boleta}'.";
                return View(nameof(Index), new BoletaCalificacionesVM { Boleta = boleta });
            }
            return View(nameof(Index), vm);
        }

        // Guarda las calificaciones capturadas (inserta o actualiza cada una).
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(BoletaCalificacionesVM modelo)
        {
            foreach (var fila in modelo.Filas)
            {
                if (fila.Valor == null) continue;

                var cal = await _db.Calificaciones
                    .FirstOrDefaultAsync(c => c.AlumnoId == modelo.AlumnoId && c.AsignacionId == fila.AsignacionId);

                if (cal == null)
                    _db.Calificaciones.Add(new Calificacion
                    {
                        AlumnoId = modelo.AlumnoId,
                        AsignacionId = fila.AsignacionId,
                        Valor = fila.Valor.Value
                    });
                else
                    cal.Valor = fila.Valor.Value;
            }
            await _db.SaveChangesAsync();

            var vm = await ConstruirVM(modelo.Boleta);
            ViewBag.Ok = "Calificaciones guardadas correctamente.";
            return View(nameof(Index), vm);
        }

        private async Task<BoletaCalificacionesVM?> ConstruirVM(string boleta)
        {
            var alumno = await _db.Alumnos.Include(a => a.Grupo)
                .FirstOrDefaultAsync(a => a.Boleta == boleta);
            if (alumno == null) return null;

            // Solo las materias en las que el alumno está inscrito (asignadas por el admin).
            var asignaciones = await _db.Inscripciones
                .Where(i => i.AlumnoId == alumno.Id)
                .Include(i => i.Asignacion!).ThenInclude(a => a.Materia)
                .Include(i => i.Asignacion!).ThenInclude(a => a.Profesor)
                .Select(i => i.Asignacion!)
                .ToListAsync();

            var califs = await _db.Calificaciones
                .Where(c => c.AlumnoId == alumno.Id).ToListAsync();

            return new BoletaCalificacionesVM
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
        }
    }
}
